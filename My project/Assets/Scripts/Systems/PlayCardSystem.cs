using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayCardSystem : MonoBehaviour
{
    public static PlayCardSystem instance;
    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        //State Machine
        states.Add(new IdleState()); //0
        states.Add(new PayCostState()); //1
        states.Add(new ResolveEffectState()); //2
        states.Add(new MoveCardState()); //3
        states.Add(new CheckAllyLimitState()); //4
        states.Add(new CardPlayedState()); //5

        ChangeState(0);
    }

    private PlayCardAction _action;
    private Player _player;

    #region State Machine
    private readonly StateMachine stateMachine = new();
    private readonly List<IState> states = new();

    public void ChangeState(int index)
    {
        stateMachine.ChangeState(states[index]);
    }
    #endregion

    #region Events
    public event UnityAction<PlayerCard> OnCardPlayed;
    #endregion

    [SerializeField] private Transform alliesTransform;
    [SerializeField] private Transform permanentsTransform;

    public void InitiatePlayCard(PlayCardAction action)
    {
        _action = action;
        _player = _action.Owner.GetComponent<Player>();
        ChangeState(1);
    }

    #region Properties
    public PlayCardAction Action
    {
        get { return _action; }
    }
    public PlayerCard CardToPlay
    {
        get { return _action.CardToPlay; }
    }
    #endregion

    #region States
    abstract class BasePlayCardSystemState : BaseState
    {
        protected PlayCardSystem owner = instance;
    }

    class PayCostState : BasePlayCardSystemState //1
    {
        public override void Enter()
        {
            owner.StartCoroutine(PayCost());
        }

        IEnumerator PayCost()
        {
            if (owner.CardToPlay.CardCost > 0)
            {
                yield return owner.StartCoroutine(PayCostSystem.instance.GetTargets(owner.CardToPlay, owner.Action.Candidates));
                List<Card> discards = PayCostSystem.instance.Discards;

                foreach (PlayerCard card in discards.Cast<PlayerCard>())
                {
                    owner._player.Hand.Remove(card);
                    owner._player.Deck.Discard(card);
                }
            }

            if (owner.CardToPlay.CardType == CardType.Event)
                owner.ChangeState(2);
            else
                owner.ChangeState(3);
        }
    }
    
    class ResolveEffectState : BasePlayCardSystemState //2
    {
        bool _activeState = false;

        public override void Enter()
        {
            _activeState = true;
            owner.CardToPlay.EnterPlay();
            ChangeState();
        }

        private void ChangeState()
        {
            if (!_activeState)
                return;

            if (owner.CardToPlay.CardType == CardType.Ally)
                owner.ChangeState(4);
            else if (owner.CardToPlay.CardType is CardType.Event)
            {
                owner._player.Hand.Remove(owner.CardToPlay);
                owner._player.Deck.Discard(owner.CardToPlay);
            }
            
            owner.ChangeState(5);
        }

        public override void Exit()
        {
            _activeState = false;
        }
    }

    class MoveCardState : BasePlayCardSystemState //3
    {
        bool _activeState = false;

        public override void Enter()
        {
            _activeState = true;

            ChangeZones(owner.CardToPlay);

            owner.CardToPlay.InPlay = true;

            owner._player.Hand.Remove(owner.CardToPlay);

            owner.CardToPlay.EnterPlay();

            ChangeState();
        }

        private void ChangeState()
        {
            if (!_activeState)
                return;

            owner.ChangeState((owner.CardToPlay is AllyCard ? 4 : 5));
        }

        private void ChangeZones(Card cardToPlay)
        {
            if (cardToPlay.CardType is CardType.Ally)
            {
                cardToPlay.transform.SetParent(owner.alliesTransform);
                owner._player.GetComponent<Player>().CardsInPlay.Allies.Add(owner.CardToPlay as AllyCard);

                cardToPlay.PrevZone = owner.CardToPlay.CurrZone;
                cardToPlay.CurrZone = Zone.Ally;
            }
            else
            {
                cardToPlay.transform.SetParent(owner.permanentsTransform);
                owner._player.GetComponent<Player>().CardsInPlay.Permanents.Add(owner.CardToPlay);

                cardToPlay.PrevZone = owner.CardToPlay.CurrZone;
                cardToPlay.CurrZone = Zone.Support;
            }
        }

        public override void Exit()
        {
            _activeState = false;
        }
    }

    class CheckAllyLimitState : BasePlayCardSystemState //4
    {
        public override void Enter()
        {
            if (owner._player.GetComponent<Player>().CardsInPlay.ReachedAllyLimit())
            {
                Debug.Log("Exceeded Ally Limit, Discard 1 Ally from Play");
                owner.StartCoroutine(DiscardAlly());
            }
            else
                owner.ChangeState(5);
        }

        IEnumerator DiscardAlly()
        {   
            List<AllyCard> allies = owner._player.GetComponent<Player>().CardsInPlay.Allies.ToList();
            allies.RemoveAll(x => x == owner.CardToPlay as AllyCard);

            yield return owner.StartCoroutine(TargetSystem.instance.SelectTarget(allies, ally =>
            {
                AllyCard target = ally;
                owner._player.Deck.Discard(target);
                owner._player.GetComponent<Player>().CardsInPlay.Allies.Remove(target);
            }));

            owner.ChangeState(5);
        }
    }

    class CardPlayedState : BasePlayCardSystemState //5
    {
        public override void Enter()
        {
            owner.OnCardPlayed?.Invoke(owner.CardToPlay);
            owner.ChangeState(0);
        }
    }

    #endregion
}
