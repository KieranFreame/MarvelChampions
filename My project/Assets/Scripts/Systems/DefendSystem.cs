using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class DefendSystem : MonoBehaviour
{
    public static DefendSystem instance;

    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        //State Machine
        States.Add(new IdleState()); //0
        States.Add(new GetTargetState()); //1
        States.Add(new ApplyDefendState()); //2

        ChangeState(0);
    }

    #region Events
    //public event UnityAction OnSelectingDefender;
    public event UnityAction OnDefenderSelected;
    #endregion

    #region StateMachine
    public StateMachine StateMachine { get; private set; } = new StateMachine();
    public List<IState> States { get; set; } = new List<IState>();

    public void ChangeState(int index)
    {
        StateMachine.ChangeState(States[index]);
    }
    #endregion

    #region Fields
    private Health _target;
    private Player _targetOwner;
    #endregion

    #region Methods

    public IEnumerator GetDefender(Player targetOwner)
    {
        _targetOwner = targetOwner;

        ChangeState(1);

        while (StateMachine.currentState != States[0])
            yield return null;
    }

    #endregion

    #region Properties
    public Health Target
    {
        get => _target;
    }
    #endregion

    #region States
    abstract class BaseDefendSystemState : BaseState
    {
        protected DefendSystem owner = instance;
    }

    class GetTargetState : BaseDefendSystemState //1
    {
        private readonly List<Health> candidates = new();

        public override void Enter()
        {
            Debug.Log("Select Defender");
                    
            foreach (AllyCard a in owner._targetOwner.GetComponent<Player>().CardsInPlay.Allies)
                candidates.Add(a.CharStats.Health);

            candidates.RemoveAll(x => x == null);
            candidates.RemoveAll(x => (x.Owner as IExhaust).Exhausted);

            if (owner._targetOwner.Identity.ActiveIdentity is Hero && !owner._targetOwner.Identity.Exhausted)
                candidates.Add(owner._targetOwner.Identity.CharStats.Health);

            if (candidates.Count > 0)
            {
                owner.StartCoroutine(GetTarget());
            }
            else
            {
                Debug.Log("No Defenders Available");
                owner.ChangeState(2);
            }   
        }

        IEnumerator GetTarget()
        {
            CancelButton.ToggleCancelBtn(true, DefenderSelectionCanceled);

            yield return owner.StartCoroutine(TargetSystem.instance.SelectTarget(candidates, health =>
            {
                owner._target = health;
            }));

            owner.ChangeState(2);
        }

        private void DefenderSelectionCanceled()
        {
            owner._target = null;
            owner.StopAllCoroutines();
            CancelButton.ToggleCancelBtn(false, DefenderSelectionCanceled);
            owner.ChangeState(2);
        }

        public override void Exit()
        {
            CancelButton.ToggleCancelBtn(false, DefenderSelectionCanceled);
            owner.OnDefenderSelected?.Invoke();
        }
    }

    /// <summary>
    /// If Hero is Defending, reduce Attack Value by Hero DEF Stat
    /// Else If Defender != Null, Set Target to Defender
    /// </summary>
    class ApplyDefendState : BaseDefendSystemState
    {
        public override void Enter()
        {
            if (owner._target != null)
            {
                if (owner._target.Owner is Identity)
                {
                    int defence = owner._target.Owner.CharStats.Defen.Defend();
                    AttackSystem.instance.Action.Value -= defence;
                }
                else
                {
                    owner._target.Owner.Exhaust();
                }
            }

            owner.ChangeState(0);
        }
    }
    #endregion
}
