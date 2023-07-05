using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class AttackSystem : MonoBehaviour //PlayerAttackSystem
{
    public static AttackSystem instance;

    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    #region Fields
    public int Excess { get; set; } = 0;
    public AttackAction Action { get; set; }
    public ICharacter Target { get; set; }
    #endregion

    #region Events
    public static event UnityAction<Action> OnAttackComplete;
    public static event UnityAction OnActivationComplete;
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
            Target.CharStats.Health.Tough = false;

        if (Action.Keywords.Contains(Keywords.Overkill))
            if (Target is not Player && Target is not Villain)
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

        await DamageSystem.instance.ApplyDamage(new DamageAction(Action, Action.Target));

        if (Excess > 0)
        {
            ICharacter overkillTarget = Action.Owner is Villain ? FindObjectOfType<Player>() : FindObjectOfType<Villain>();
            await DamageSystem.instance.ApplyDamage(new DamageAction(overkillTarget, Excess));
        }

        OnActivationComplete?.Invoke();
        OnAttackComplete?.Invoke(Action);
    }

    private async Task FriendlyAttack()
    {
        List<ICharacter> enemies = new() { FindObjectOfType<Villain>() };
        enemies.AddRange(FindObjectsOfType<MinionCard>());

        if (enemies.Count > 1)
        {
            Action.Target = await TargetSystem.instance.SelectTarget(enemies, true);
            return;
        }

        Action.Target = enemies[0];
        TargetSystem.SingleTarget(enemies[0]);
    }

    private async Task EnemyAttack()
    {
        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
        {
            BoostSystem.instance.DealBoostCards();
        }

        Action.Target = await DefendSystem.instance.GetDefender(TurnManager.instance.CurrPlayer);

        if (Action.Target == null)
            Action.Target = TurnManager.instance.CurrPlayer;

        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
           Action.Value += await BoostSystem.instance.FlipCard(Action);
    }
    #endregion
}