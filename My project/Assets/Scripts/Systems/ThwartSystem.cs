using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThwartSystem : MonoBehaviour
{
    public static ThwartSystem instance;

    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        //State Machine
        states.Add(new IdleState()); //0
        states.Add(new GetTargetState()); //1
        states.Add(new SendThwartState()); //2
        states.Add(new ThwartCompletedState()); //3

        ChangeState(0);
    }

    #region State Machine
    private readonly StateMachine stateMachine = new();
    private readonly List<IState> states = new();

    public void ChangeState(int index)
    {
        stateMachine.ChangeState(states[index]);
    }
    #endregion

    #region Events
    public static event UnityAction OnThwartComplete;
    #endregion

    #region Fields
    private ThwartAction _action;
    private Threat _target;
    #endregion

    public static void InitiateThwart(ThwartAction action)
    {
        instance._action = action;
        instance.ChangeState(1);
    }

    #region Properties
    public ThwartAction ThwartAction
    {
        get => _action;
    }

    public Threat Target
    {
        get => _target;
        set { _target = value; }
    }
    #endregion

    #region States

    abstract class BaseThwartSystemState : BaseState
    {
        protected ThwartSystem owner = instance;
    }

    class GetTargetState : BaseThwartSystemState
    {
        public override void Enter()
        {
            owner.StartCoroutine(GetTarget());
        }

        IEnumerator GetTarget()
        {
            yield return owner.StartCoroutine(TargetSystem.instance.GetTarget<Threat>(owner.ThwartAction, threat => { owner.Target = threat; }));
            owner.ChangeState(2);
        }
    }

    class SendThwartState : BaseThwartSystemState
    {
        public override void Enter()
        {
            owner.Target.RemoveThreat(owner.ThwartAction.Value);
            owner.ChangeState(3);
        }
    }

    class ThwartCompletedState : BaseThwartSystemState
    {
        public override void Enter()
        {
            OnThwartComplete?.Invoke();
            owner.ChangeState(0);
        }
    }

    #endregion
}
