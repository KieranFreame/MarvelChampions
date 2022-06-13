using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillainTurnController : MonoBehaviour
{
    public static VillainTurnController instance;
    List<IState> states = new List<IState>();
    StateMachine stateMachine = new StateMachine();

    #region GameEvents
    public GameEvent accelerate;
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        /*states.Add(new VillainIdleState());
        states.Add(new AccelerateState());
        states.Add(new VillainActivateState());
        states.Add(new DealEncounterCardState());
        states.Add(new RevealEncounterCardState());
        states.Add(new RotateFirstPlayerState());*/

        stateMachine.ChangeState(states[0]);
    }

    public void StartTurn()
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
        protected VillainTurnController owner = VillainTurnController.instance;
    }

    class VillainIdleState : BaseControllerState
    {
        
    }

    /*class AccelerateState : BaseControllerState
    {
        public override void Execute()
        {
            owner.scenario.mainScheme.Accelerate();
            owner.stateMachine.ChangeState(owner.states[2]);
        }
    }

    class VillainActivateState : BaseControllerState
    {
        public override void Execute()
        {
            foreach (Player p in GameManager.instance.players)
            {
                if (p.activeIdentity is AlterEgo)
                {
                    scen.villain.Scheme();

                    foreach (Minion m in scen.activeMinions)
                    {
                        m.Scheme();
                    }
                }
                else
                {
                    scen.villain.Attack(p);

                    foreach (Minion m in scen.activeMinions)
                    {
                        m.Attack(p);
                    }
                }
            }
            owner.stateMachine.ChangeState(owner.states[3]);
        }

    }

    class DealEncounterCardState : BaseControllerState
    {
        public override void Execute()
        {
            int hazardCount = GameManager.instance.players.Count;

            foreach (SideScheme s in scen.activeSideSchemes)
            {
                if (s is IHazard)
                {
                    hazardCount++;
                }
            }

            int cardsDealt = 0;

            foreach (Player p in GameManager.instance.players)
            {
                p.encounterCards.Add(scen.encounterDeck.Deal());
                cardsDealt++;

                if (cardsDealt == hazardCount)
                    break;
            }

            owner.stateMachine.ChangeState(owner.states[4]);
        }
    }

    class RevealEncounterCardState : BaseControllerState
    {
        public override void Execute()
        {
            foreach (Player p in GameManager.instance.players)
            {
                p.RevealEncounterCards();
            }

            owner.stateMachine.ChangeState(owner.states[5]);
        }
    }

    class RotateFirstPlayerState : BaseControllerState
    {
        public override void Execute()
        {
            TurnManager.instance.ChangeFirstPlayer();
            owner.stateMachine.ChangeState(owner.states[0]);
        }
    }*/
    #endregion
}
