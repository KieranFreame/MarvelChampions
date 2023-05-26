using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VillainTurnController : MonoBehaviour
{
    public static VillainTurnController instance;

    #region StateMachine
    private readonly List<IState> states = new();
    private readonly StateMachine stateMachine = new();

    public void ChangeState(int index)
    {
        stateMachine.ChangeState(states[index]);
    }
    #endregion

    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        _owner = FindObjectOfType<Villain>().GetComponent<Villain>();

        //State Machine
        states.Add(new VillainIdleState()); //0
        states.Add(new AccelerateState()); //1
        states.Add(new VillainActivateState()); //2
        states.Add(new MinionActivateState()); //3
        states.Add(new DealEncounterCardState()); //4
        states.Add(new RevealEncounterCardState()); //5
        //states.Add(new RotateFirstPlayerState()); //6

        stateMachine.ChangeState(states[0]);
    }

    private void OnEnable()
    {
        TurnManager.OnStartVillainPhase += StartVillainPhase;
    }

    private void OnDisable()
    {
        TurnManager.OnStartVillainPhase -= StartVillainPhase;
    }

    public int HazardCount { get; set; }

    #region Events
    public event UnityAction Accelerate;
    //public event UnityAction VillainAttack;
    //public event UnityAction VillainScheme;

    public event UnityAction OnRevealCards;
    #endregion

    #region Fields
    [SerializeField] private Transform _minionTransform;
    public int EncounterCardCount { get; set; }
    private Villain _owner;
    #endregion

    public Transform MinionTransform
    {
        get => _minionTransform;
    }

    public void StartVillainPhase()
    {
        stateMachine.ChangeState(states[1]);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    #region States
    abstract class BaseControllerState : BaseState
    {
        protected VillainTurnController owner = instance;
        protected Villain villain = instance._owner;
    }
    class VillainIdleState : BaseControllerState //0
    {
        public override void Enter()
        {
            TurnManager.instance.EndVillainPhase();
        }

        public override void Exit()
        {
            Debug.Log("Villain Phase Has Started");
        }
    } 
    class AccelerateState : BaseControllerState //1
    {
        public override void Enter()
        {
            Debug.Log("Accelerating");
            owner.Accelerate?.Invoke();
            owner.ChangeState(2);
        }
    }
    class VillainActivateState : BaseControllerState //2
    {
        bool activating;

        public override void Enter()
        {
            Debug.Log("Villain is Activating");

            AttackSystem.OnActivationComplete += ActivationComplete;
            SchemeSystem.OnActivationComplete += ActivationComplete;

            foreach (Player p in TurnManager.Players)
            {
                owner.StartCoroutine(p.Identity.ActiveIdentity is AlterEgo ? VillainScheme() : VillainAttack());
            }
        }

        private IEnumerator VillainScheme()
        {
            activating = true;

            //villain.InitiateScheme();

            while (activating)
                yield return null;

            if (owner.MinionTransform.childCount > 0)
                owner.ChangeState(3);
            else
                owner.ChangeState(4);
        }
        
        private IEnumerator VillainAttack()
        {
            activating = true;

            //villain.InitiateAttack();

            while (activating)
                yield return null;

            if (owner.MinionTransform.childCount > 0)
                owner.ChangeState(3);
            else
                owner.ChangeState(4);
        }

        private void ActivationComplete() => activating = false;

        public override void Exit()
        {
            AttackSystem.OnActivationComplete -= ActivationComplete;
            SchemeSystem.OnActivationComplete -= ActivationComplete;
            
        }

    }
    class MinionActivateState : BaseControllerState //3
    {
        bool activating;

        public override void Enter()
        {
            Debug.Log("Minions are Activating");

            AttackSystem.OnActivationComplete += ActivationComplete;
            SchemeSystem.OnActivationComplete += ActivationComplete;

            List<MinionCard> minions = new();
            minions.AddRange(owner.MinionTransform.GetComponentsInChildren<MinionCard>());

            foreach (Player p in TurnManager.Players)
            {
                if (p.Identity.ActiveIdentity is AlterEgo)
                    owner.StartCoroutine(MinionScheme(minions));
                else
                    owner.StartCoroutine(MinionAttack(minions));                 
            }
            
        }

        private IEnumerator MinionScheme(List<MinionCard> minions)
        {
            foreach (MinionCard m in minions)
            {
                activating = true;

                //m.InitiateScheme();

                while (activating)
                    yield return null;
            }

            owner.ChangeState(4);
        }
        
        private IEnumerator MinionAttack(List<MinionCard> minions)
        {
            foreach (MinionCard m in minions)
            {
                activating = true;

                //m.InitiateAttack();

                while (activating)
                    yield return null;
            }

            owner.ChangeState(4);
        }

        private void ActivationComplete() => activating = false;

        public override void Exit()
        {
            AttackSystem.OnActivationComplete -= ActivationComplete;
            SchemeSystem.OnActivationComplete -= ActivationComplete;
        }
    }
    class DealEncounterCardState : BaseControllerState //4
    {
        public override void Enter()
        {
            Debug.Log("Dealing Encounter Cards");
            
            owner.EncounterCardCount = TurnManager.Players.Count + owner.HazardCount;

            while (owner.EncounterCardCount > 0)
            {
                foreach (Player p in TurnManager.Players)
                {
                    //p.EncounterCards.AddCards(owner.GetComponentInChildren<Villain>().EncounterDeck.deck[0]);
                    owner.GetComponentInChildren<Villain>().EncounterDeck.Deal();
                    owner.EncounterCardCount--;

                    if (owner.EncounterCardCount == 0)
                        break;
                }
            }

            owner.ChangeState(5);
        }
    }

    class RevealEncounterCardState : BaseControllerState //5
    {
        public override void Enter()
        {
            Debug.Log("Revealing Encounter Cards");

            owner.OnRevealCards?.Invoke();
            owner.ChangeState(0); //temp, 6
        }
    }

    /* class RotateFirstPlayerState : BaseControllerState //6
    {
        public override void Execute()
        {
            TurnManager.instance.ChangeFirstPlayer();
            owner.ChangeState(0);
        }
    }*/
    #endregion
}
