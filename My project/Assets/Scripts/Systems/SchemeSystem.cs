using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SchemeSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static SchemeSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        _states.Add(new IdleState());
        _states.Add(new ResetState());
        _states.Add(new DealBoostCardState());
        _states.Add(new FlipBoostState());
        _states.Add(new ApplyThreatState());
        _states.Add(new SchemeCompletedState());
    }
    #endregion

    #region StateMachine
    private readonly StateMachine _stateMachine = new();
    private readonly List<IState> _states = new();

    public void ChangeState(int index)
    {
        _stateMachine.ChangeState(_states[index]);
    }
    #endregion

    #region Events
    public static event UnityAction<Action> OnSchemeComplete;
    public static event UnityAction OnActivationComplete;
    #endregion

    #region Fields
    private SchemeAction _action;
    private Threat _target;
    #endregion

    public void InitiateScheme(SchemeAction action)
    {
        _action = action;
        ChangeState(1);
    }

    #region States
    abstract class BaseSchemeState : BaseState
    {
        protected SchemeSystem owner = instance;
    }

    class ResetState : BaseSchemeState //1
    {
        public override void Enter()
        {
            owner._target = null;

            if (owner._action.Owner.GetComponent<Villain>() != null || owner._action.Keywords.Contains(Keywords.Villainous))
                owner.ChangeState(2);
            else
                owner.ChangeState(4);
        }
    }

    class DealBoostCardState : BaseSchemeState //2
    {
        public override void Enter()
        {
            BoostSystem.instance.BoostCardCount = 1; //temp
            BoostSystem.instance.DealBoostCards();
            owner.ChangeState(3);
        }
    }

    class FlipBoostState : BaseSchemeState //3
    {
        public override void Enter()
        {
            BoostSystem.OnBoostCardsResolved += BoostCardsResolved;
            BoostSystem.instance.FlipBoostCards();
        }

        private void BoostCardsResolved(int actionValue)
        {
            owner._action.Value += actionValue;
            BoostSystem.OnBoostCardsResolved -= BoostCardsResolved;
            owner.ChangeState(4);
        }
    }

    class ApplyThreatState : BaseSchemeState //4
    {
        public override void Enter()
        {
            owner._target = FindObjectOfType<MainSchemeCard>().GetComponent<Threat>();
            Debug.Log(owner._action.Owner.name + " is placing " + owner._action.Value + " threat on the main scheme");
            owner._target.GainThreat(owner._action.Value);
            owner.ChangeState(5);
        }
    }

    class SchemeCompletedState : BaseSchemeState //5
    {
        public override void Enter()
        {
            OnActivationComplete?.Invoke();
            OnSchemeComplete?.Invoke(owner._action);
            owner.ChangeState(0);
        }
    }
    #endregion
}
