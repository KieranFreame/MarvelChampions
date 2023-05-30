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
    public IEnumerator InitiateAttack(AttackAction action)
    {
        Action = action;

        ResetSystem();

        if (Target == null)
            yield return StartCoroutine(FriendlyAttack());
        else
            yield return StartCoroutine(EnemyAttack());

        CheckKeywords();

        yield return StartCoroutine(DamageSystem.instance.ApplyDamage(new DamageAction(Action, Target)));

        if (Excess > 0)
        {
            ICharacter overkillTarget = Action.Owner is Villain ? FindObjectOfType<Player>() : FindObjectOfType<Villain>();
            yield return StartCoroutine(DamageSystem.instance.ApplyDamage(new DamageAction(overkillTarget, Excess)));
        }

        OnActivationComplete?.Invoke();
        OnAttackComplete?.Invoke(Action);
    }

    private IEnumerator FriendlyAttack()
    {
        List<ICharacter> enemies = new() { FindObjectOfType<Villain>() };
        enemies.AddRange(FindObjectsOfType<MinionCard>());

        if (enemies.Count > 1)
        {
            yield return StartCoroutine(TargetSystem.instance.SelectTarget(enemies, target =>
            {
                Target = target;
            }, true));

            yield break;
        }

        Target = enemies[0];
    }

    private IEnumerator EnemyAttack()
    {
        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
        {
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