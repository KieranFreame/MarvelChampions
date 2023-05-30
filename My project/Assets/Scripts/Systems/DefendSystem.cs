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
    public ICharacter Target { get; set; }
    private Player _targetOwner;
    #endregion

    #region Methods

    public IEnumerator GetDefender(Player targetOwner, System.Action<ICharacter> callback)
    {
        _targetOwner = targetOwner;

        ChangeState(1);

        while (StateMachine.currentState != States[0])
            yield return null;

        callback(Target);
    }

    #endregion

    #region States
    abstract class BaseDefendSystemState : BaseState
    {
        protected DefendSystem owner = instance;
    }

    class GetTargetState : BaseDefendSystemState //1
    {
        private readonly List<ICharacter> candidates = new();

        public override void Enter()
        {
            Debug.Log("Select Defender");

            candidates.AddRange(owner._targetOwner.GetComponent<Player>().CardsInPlay.Allies);

            candidates.RemoveAll(x => x == null);
            candidates.RemoveAll(x => (x as AllyCard).Exhausted);

            if (owner._targetOwner.Identity.ActiveIdentity is Hero && !owner._targetOwner.Identity.Exhausted)
                candidates.Add(owner._targetOwner);

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

            yield return owner.StartCoroutine(TargetSystem.instance.SelectTarget(candidates, character =>
            {
                owner.Target = character;
            }));

            owner.ChangeState(2);
        }

        private void DefenderSelectionCanceled()
        {
            owner.Target = null;
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
            if (owner.Target != null)
            {
                if (owner.Target is Player)
                {
                    int defence = owner.Target.CharStats.Defender.Defend();
                    AttackSystem.instance.Action.Value -= defence;
                }
                else
                {
                    (owner.Target as AllyCard).Exhaust();
                }
            }

            owner.ChangeState(0);
        }
    }
    #endregion
}
