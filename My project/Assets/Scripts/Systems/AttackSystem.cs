using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

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
    public Health Target { get; set; }
    #endregion

    #region Events
    public static event UnityAction<Action> OnAttackComplete;
    public static event UnityAction OnActivationComplete;
    #endregion

    #region Methods
    private void ResetSystem()
    {
        if (Action.Owner is Villain || Action.Owner is MinionCard)
            Target = TurnManager.instance.CurrPlayer.CharStats.Health;
        else
            Target = null;

        Excess = 0;
    }
    private void CheckKeywords()
    {
        if (Action.Keywords.Contains(Keywords.Piercing))
            Target.Tough = false;

        if (Action.Keywords.Contains(Keywords.Overkill))
            if (Target.Owner is not Identity && Target.Owner is not Villain)
                Excess = Action.Value - Target.CurrentHealth;
    }
    #endregion

    #region Coroutines
    public IEnumerator InitiateAttack(AttackAction action)
    {
        Action = action;

        ResetSystem();

        if (Target == null)
            yield return StartCoroutine(FriendlyAttack());
        else
            yield return StartCoroutine(EnemyAttack());

        CheckKeywords();

        yield return StartCoroutine(DamageSystem.ApplyDamage(new DamageAction(Action, Target)));

        if (Excess > 0)
        {
            var overkillTarget = Action.Owner is Villain ? FindObjectOfType<Player>().CharStats.Health : FindObjectOfType<Villain>().CharStats.Health;
            yield return StartCoroutine(DamageSystem.ApplyDamage(new DamageAction(overkillTarget, Excess)));
        }

        OnActivationComplete?.Invoke();
        OnAttackComplete?.Invoke(Action);
    }

    private IEnumerator FriendlyAttack()
    {
        List<ICharacter> targets = new() { FindObjectOfType<Villain>() };
        targets.AddRange(FindObjectsOfType<MinionCard>());
        yield return StartCoroutine(TargetSystem.instance.SelectTarget(targets, target =>
        {
            Target = target.CharStats.Health;
        }));
    }

    private IEnumerator EnemyAttack()
    {
        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
        {
            BoostSystem.instance.BoostCardCount = 1;
            BoostSystem.instance.DealBoostCards();
        }

        yield return StartCoroutine(DefendSystem.instance.GetDefender(TurnManager.instance.CurrPlayer, defender =>
        {
            if (defender != null)
                Target = defender;
        }));

        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
            yield return BoostSystem.instance.FlipCard(boost => { Action.Value += boost; });
    }
    #endregion
}