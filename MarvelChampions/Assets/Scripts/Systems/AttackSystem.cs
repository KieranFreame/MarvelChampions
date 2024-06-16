using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class AttackSystem
{
    private static AttackSystem instance;

    public static AttackSystem Instance
    {
        get
        {
            instance ??= new AttackSystem();
            return instance;
        }
    }

    AttackSystem()
    {
        PauseMenu.OnRestartGame += Restart;
    }

    private void Restart()
    {
        PauseMenu.OnRestartGame -= Restart;
        instance = null;
    }

    #region Fields
    public int Excess { get; set; } = 0;
    public AttackAction Action { get; set; }
    public ICharacter Target { get; set; }
    public List<MinionCard> Guards { get; set; } = new();
    #endregion

    public delegate void AttackComplete(AttackAction action);
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
                Excess = Action.Value - Action.Target.CharStats.Health.CurrentHealth;
    }
    #endregion

    #region Coroutines
    public async Task InitiateAttack(AttackAction action)
    {
        Action = action;

        ResetSystem();

        if (Target == null)
            await PlayerAttack();
        else
            await EnemyAttack();

        CheckKeywords();

        await DamageSystem.Instance.ApplyDamage(new(Action.Target, Action.Value, isAttack: true, card: action.Card, owner:Action.Owner));

        if (Excess > 0)
        {
            ICharacter overkillTarget = Action.Owner == ScenarioManager.inst.ActiveVillain as ICharacter ? TurnManager.instance.CurrPlayer : ScenarioManager.inst.ActiveVillain;
            await DamageSystem.Instance.ApplyDamage(new DamageAction(overkillTarget, Excess, card: action.Card));
        }

        GameStateManager.Instance.ActivationCompleted(action);
        await EffectManager.Inst.CheckResponding();
    }

    private async Task PlayerAttack()
    {
        List<ICharacter> enemies = new();
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        if (Guards.Count == 0 || !Action.Targets.Contains(TargetType.TargetVillain))
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

        Action.Target = await DefendSystem.Instance.GetDefender(TurnManager.instance.CurrPlayer, Action);

        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
           Action.Value += await BoostSystem.Instance.FlipCard(Action);
    }
    #endregion
}