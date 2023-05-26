using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

        //State Machine
        states.Add(new IdleState()); //0
        states.Add(new ResetState()); //1
        states.Add(new GetTargetState()); //2
        states.Add(new DealBoostCardState()); //3
        states.Add(new WaitForDefenderState()); //4
        states.Add(new FlipBoostState()); //5
        states.Add(new CheckKeywordsState()); //6
        states.Add(new SendDamageState()); //7
        states.Add(new ApplyOverkillState()); //8
        states.Add(new AttackCompletedState()); //9
        
        ChangeState(0);
    }

    #region Fields
    private int _excess = 0;
    public AttackAction Action { get; set; }
    private Health _target;
    private readonly StateMachine stateMachine = new();
    private readonly List<IState> states = new();
    #endregion

    #region Events
    public static event UnityAction<Action> OnAttackComplete;
    public static event UnityAction OnActivationComplete;
    #endregion

    #region Methods
    public void InitiateAttack(AttackAction action)
    {
        Action = action;
        ChangeState(1);
    }

    public void ChangeState(int index)
    {
        stateMachine.ChangeState(states[index]);
    }
    #endregion

    #region Properties
    public Health Target
    {
        get { return _target; }
        set { _target = value; }
    }
    public int Excess
    {
        get { return _excess; }
        set { _excess = value; }
    }
    #endregion

    #region States
    abstract class BaseAttackSystemState : BaseState
    {
        protected AttackSystem owner = instance;
    }
    class ResetState : BaseAttackSystemState //1
    {
        public override void Enter()
        {
            if (owner.Action.Owner is not Villain && owner.Action.Owner is not MinionCard)
                owner.Target = null;
            else
                TurnManager.instance.CurrPlayer.TryGetComponent(out owner._target);

            owner.Excess = 0;

            if (owner.Target == null) // Hero/Ally is attacking
                owner.ChangeState(2);
            else if (owner.Action.Owner is Villain || owner.Action.Keywords.Contains(Keywords.Villainous))
                owner.ChangeState(3);
            else //Minion w/o Villainous
                owner.ChangeState(4);
        }
    }
    class GetTargetState : BaseAttackSystemState //2
    {
        public override void Enter()
        {
            owner.StartCoroutine(GetTarget());
        }

        private IEnumerator GetTarget()
        {
            yield return owner.StartCoroutine(TargetSystem.instance.GetTarget<Health>(owner.Action, health => { owner.Target = health; }));
            owner.ChangeState(6);
        }
    }

    class DealBoostCardState : BaseAttackSystemState //3
    {
        public override void Enter()
        {
            BoostSystem.instance.BoostCardCount = 1;
            BoostSystem.instance.DealBoostCards();
            owner.ChangeState(4);
        }
    }
    class WaitForDefenderState : BaseAttackSystemState //4
    {
        public override void Enter()
        {
            owner.StartCoroutine(GetDefender());
        }

        private IEnumerator GetDefender()
        {
            yield return owner.StartCoroutine(DefendSystem.instance.GetDefender(TurnManager.instance.CurrPlayer));

            if (DefendSystem.instance.Target != null)
                owner.Target = DefendSystem.instance.Target;

            if (owner.Action.Owner is Villain || owner.Action.Keywords.Contains(Keywords.Villainous))
                owner.ChangeState(5);
            else
                owner.ChangeState(6);
        }
    }
    class FlipBoostState : BaseAttackSystemState //5
    {
        public override void Enter()
        {
            BoostSystem.OnBoostCardsResolved += BoostCardsResolved;
            BoostSystem.instance.FlipBoostCards();
        }

        private void BoostCardsResolved(int actionValue)
        {
            owner.Action.Value += actionValue;
            BoostSystem.OnBoostCardsResolved -= BoostCardsResolved;
            owner.ChangeState(6);
        }
    }

    class CheckKeywordsState : BaseAttackSystemState //6
    {
        public override void Enter()
        {
            if (owner.Action.Keywords.Contains(Keywords.Piercing))
                owner.Target.Tough = false;

            if (owner.Action.Keywords.Contains(Keywords.Overkill))
                if (owner.Target.Owner is not Player && owner.Target.Owner is not Villain)
                    owner.Excess = owner.Action.Value - owner.Target.CurrentHealth;

            owner.ChangeState(7);
        }
    }
    class SendDamageState : BaseAttackSystemState //7
    {
        public override void Enter()
        {
            DamageSystem.OnDamageApplied += ChangeState;
            owner.StartCoroutine(DamageSystem.ApplyDamage(new DamageAction(owner.Action, owner.Target)));
        }

        public override void Exit()
        {
            DamageSystem.OnDamageApplied -= ChangeState;
        }

        private void ChangeState()
        {
            owner.ChangeState((owner.Excess > 0) ? 8 : 9);
        }
    }
    class ApplyOverkillState : BaseAttackSystemState //8
    {
        public override void Enter()
        {
            DamageSystem.OnDamageApplied += ChangeState;

            var overkillTarget = owner.Action.Owner is Villain ? FindObjectOfType<Player>().GetComponent<Health>() : FindObjectOfType<Villain>().GetComponent<Health>();
            owner.StartCoroutine(DamageSystem.ApplyDamage(new DamageAction(overkillTarget, owner.Excess)));
        }

        public override void Exit()
        {
            DamageSystem.OnDamageApplied -= ChangeState;
        }

        private void ChangeState()
        {
            owner.ChangeState(9);
        }
    }
    class AttackCompletedState : BaseAttackSystemState //9
    {
        public override void Enter()
        {
            OnActivationComplete?.Invoke();
            OnAttackComplete?.Invoke(owner.Action);
            owner.ChangeState(0);
        }
    }
    #endregion
}
