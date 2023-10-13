using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class AttackSystem
{
    private static AttackSystem instance = new();

    public static AttackSystem Instance
    {
        get => instance;
    }

    #region Fields
    public int Excess { get; set; } = 0;
    public AttackAction Action { get; set; }
    public ICharacter Target { get; set; }
    public List<MinionCard> Guards { get; set; } = new();
    #endregion

    public delegate Task AttackComplete(AttackAction action);
    public List<AttackComplete> OnAttackCompleted { get; private set; } = new();

    #region Events
    public static event UnityAction<ICharacter> TargetAcquired;
    #endregion

    #region Methods
    private void ResetSystem()
    {
        if (Action.Owner is Villain || Action.Owner is MinionCard)
            Target = TurnManager.instance.CurrPlayer;
        else
            Target = null;

        Excess = 0;
    }
    private void CheckKeywords()
    {
        if (Action.Keywords.Contains(Keywords.Piercing))
            Action.Target.CharStats.Health.Tough = false;

        if (Action.Keywords.Contains(Keywords.Overkill))
            if (Action.Target is not Player && Action.Target is not Villain)
                Excess = Action.Value - Target.CharStats.Health.CurrentHealth;
    }
    #endregion

    #region Coroutines
    public async Task InitiateAttack(AttackAction action)
    {
        Action = action;

        ResetSystem();

        if (Target == null)
            await FriendlyAttack();
        else
            await EnemyAttack();

        CheckKeywords();

        await DamageSystem.Instance.ApplyDamage(new(Action.Target, Action.Value, isAttack: true, card: action.Card, owner:Action.Owner));

        if (Excess > 0)
        {
            ICharacter overkillTarget = Action.Owner is Villain ? (Action.Target as AllyCard).Owner : (Action.Target as MinionCard).Owner;
            await DamageSystem.Instance.ApplyDamage(new DamageAction(overkillTarget, Excess, card: action.Card));
        }

        for (int i = OnAttackCompleted.Count -1; i >= 0; i--)
        {
            await OnAttackCompleted[i](Action);
        }
    }

    private async Task FriendlyAttack()
    {
        List<ICharacter> enemies = new();
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        if (Guards.Count == 0)
            enemies.Add(ScenarioManager.inst.ActiveVillain);

        if (enemies.Count > 1)
            Action.Target = await TargetSystem.instance.SelectTarget(enemies);
        else
            Action.Target = enemies[0];
        
        TargetAcquired?.Invoke(Action.Target);
    }

    private async Task EnemyAttack()
    {
        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
        {
            BoostSystem.Instance.DealBoostCards();
        }

        Action.Target = await DefendSystem.Instance.GetDefender(TurnManager.instance.CurrPlayer);

        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
           Action.Value += await BoostSystem.Instance.FlipCard(Action);
    }
    #endregion
}