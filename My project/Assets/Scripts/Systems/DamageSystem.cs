using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class DamageSystem : MonoBehaviour
{
    private void Awake()
    {
        //Singleton Pattern
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        //State Machine
        states.Add(new IdleState());
        states.Add(new CheckTargetState());
        states.Add(new ApplyDamageModifiersState());
        states.Add(new ApplyDamageState());

        ChangeState(0);
    }

    #region Fields
    //Singleton
    public static DamageSystem instance;

    //StateMachine
    private readonly StateMachine stateMachine = new();
    private readonly List<IState> states = new();

    public DamageAction DamageAction { get; private set; }
    #endregion

    #region Events
    public static event UnityAction OnDamageApplied;
    #endregion

    #region Delegates
    public List<IModifyDamage> Modifiers { get; private set; } = new List<IModifyDamage>();
    #endregion

    public static IEnumerator ApplyDamage(DamageAction action)
    {
        instance.DamageAction = action;
        instance.ChangeState(1);

        while (instance.stateMachine.currentState != instance.states[0])
            yield return null;
    }

    public void ChangeState(int index)
    {
        stateMachine.ChangeState(states[index]);
    }

    #region States
    abstract class BaseDamageSystemState : BaseState
    {
        protected DamageSystem owner = instance;
    }

    class CheckTargetState : BaseDamageSystemState //1
    {
        public override void Enter()
        {
            if (owner.DamageAction.DamageTargets.Count > 1 && owner.DamageAction.TargetAll || owner.DamageAction.DamageTargets.Count == 1)
                owner.ChangeState(2);
            else
                owner.StartCoroutine(SelectTarget());
        }

        IEnumerator SelectTarget()
        {
            yield return owner.StartCoroutine(TargetSystem.instance.SelectTarget(owner.DamageAction.DamageTargets, health =>
            {
                owner.DamageAction.DamageTargets.Add(health);
            }));
            owner.ChangeState(2);
        }
    }

    class ApplyDamageModifiersState : BaseDamageSystemState //2
    {
        public override void Enter()
        {
            owner.StartCoroutine(ApplyModifiers());
        }

        public IEnumerator ApplyModifiers()
        {
            for (int i = owner.Modifiers.Count - 1; i >= 0; i--)
            {
                if (owner.Modifiers[i] == null)
                {
                    owner.Modifiers.RemoveAt(i);
                    continue;
                }

                yield return (owner.StartCoroutine(owner.Modifiers[i].OnTakeDamage(owner.DamageAction, action =>
                {
                    if (action.Value < 0) action.Value = 0;
                    owner.DamageAction = action;
                })));
            }

            owner.ChangeState(3);
        }

    }

    class ApplyDamageState : BaseDamageSystemState //3
    {
        public override void Enter()
        {
            foreach (Health h in owner.DamageAction.DamageTargets)
                h.TakeDamage(owner.DamageAction.Value);

            owner.ChangeState(0);
        }

        public override void Exit()
        {
            OnDamageApplied?.Invoke();
        }
    }
    #endregion
}

