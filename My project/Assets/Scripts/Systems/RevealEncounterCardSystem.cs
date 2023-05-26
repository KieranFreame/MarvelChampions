using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RevealEncounterCardSystem : MonoBehaviour
{
    public static RevealEncounterCardSystem instance;

    private void Awake()
    {
        //Singleton
        if (instance == null) { instance = this; }
        else { Destroy(this); }

        //State Machine
        _states.Add(new IdleState());
        _states.Add(new CardTypeState());
        _states.Add(new MoveCardState());
        _states.Add(new ResolveEffectState());
        _states.Add(new CardRevealedState());

        ChangeState(0);
    }

    #region State Machine
    private readonly StateMachine _stateMachine = new();
    private readonly List<IState> _states = new();
    public void ChangeState(int index)
    {
        _stateMachine.ChangeState(_states[index]);
    }
    #endregion

    #region Fields
    private EncounterCard _cardToPlay;
    #endregion

    #region Transforms
    [SerializeField] private Transform _minionTransform;
    [SerializeField] private Transform _sideSchemeTransform;
    [SerializeField] private Transform _attachmentTransform;
    #endregion

    #region Events
    public static event UnityAction<EncounterCard> OnEncounterCardRevealed;
    #endregion

    #region Methods
    public void InitiateRevealCard(EncounterCard cardToPlay)
    {
        _cardToPlay = cardToPlay;
        Debug.Log("Revealing " + cardToPlay.CardName);
        ChangeState(1);
    }
    #endregion

    #region States
    abstract class BaseRevealECState : BaseState
    {
        protected RevealEncounterCardSystem owner = instance;
    }

    class CardTypeState : BaseRevealECState //1
    {
        public override void Enter()
        {
            owner.StartCoroutine(RevealCard());
        }

        IEnumerator RevealCard()
        {
            yield return new WaitForSeconds(3);

            if (owner._cardToPlay.CardType is CardType.Treachery)
                owner.ChangeState(3);
            else
                owner.ChangeState(2);
        }
    }

    class MoveCardState : BaseRevealECState //2
    {
        public override void Enter()
        {
            switch (owner._cardToPlay.CardType)
            {
                case CardType.Minion:
                    owner._cardToPlay.transform.SetParent(owner._minionTransform);
                    break;
                case CardType.Scheme:
                    owner._cardToPlay.transform.SetParent(owner._sideSchemeTransform);
                    ScenarioManager.sideSchemes.Add(owner._cardToPlay as SchemeCard);
                    break;
                case CardType.Attachment:
                    owner._cardToPlay.transform.SetParent(owner._attachmentTransform);
                    break;
            }

            owner.ChangeState(3);
        }
    }

    class ResolveEffectState : BaseRevealECState //3
    {
        public override void Enter()
        {
            owner._cardToPlay.OnRevealCard();
            ChangeState();
        }

        private void ChangeState()
        {
            if (owner._cardToPlay.CardType == CardType.Treachery)
            {
                FindObjectOfType<Villain>().EncounterDeck.Discard(owner._cardToPlay);
            }

            owner.ChangeState(4);
        }    
        
    }

    class CardRevealedState : BaseRevealECState //4
    {
        public override void Enter()
        {
            OnEncounterCardRevealed?.Invoke(owner._cardToPlay);
            owner.ChangeState(0);
        }
    }
    #endregion
}
