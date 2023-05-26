using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class PayCostSystem : MonoBehaviour
{
    #region SingletonPattern
    public static PayCostSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        states.Add(new IdleState());
        states.Add(new GetCandidatesState());
        states.Add(new SelectTargetsState());

        stateMachine.ChangeState(states[0]);
    }
    #endregion

    #region Pointer
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    void Start()
    {
        raycaster = GameObject.Find("Board").GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }
    #endregion

    #region State Machine
    private readonly StateMachine stateMachine = new();
    private readonly List<IState> states = new();

    public void ChangeState(int index)
    {
        stateMachine.ChangeState(states[index]);
    }
    #endregion

    #region Fields
    readonly List<PlayerCard> candidates = new();
    readonly private List<Card> _discards = new();
    private readonly List<Resource> _resources = new();
    PlayerCard cardToPlay;
    List<PlayerCard> zone;

    private bool finishedSelecting = false;
    #endregion

    private void Update()
    {
        stateMachine.Update();
    }

    private void Clear()
    {
        Resources.Clear();
        Discards.Clear();
    }

    public IEnumerator GetTargets(PlayerCard card, List<PlayerCard> targets)
    {
        cardToPlay = card;
        zone = targets;
        stateMachine.ChangeState(states[1]);

        while (stateMachine.currentState != states[0])
            yield return null;
    }

    public IEnumerator GetResources(Resource resourceToCheck = Resource.Wild, int amount = 0)
    {
        UIManager.InStateMachine = true;
        FinishButton.ToggleFinishButton(true, FinishedSelecting);
        PlayerCard card = null;
        Player player = FindObjectOfType<Player>();
        finishedSelecting = false;
        Clear();

        while (!finishedSelecting)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                pointerEventData = new PointerEventData(eventSystem)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new();

                raycaster.Raycast(pointerEventData, results);

                results.Find(x => x.gameObject.TryGetComponent(out card));

                if (card != null)
                {
                    if (card.Resources.Contains(resourceToCheck) || card.Resources.Contains(Resource.Wild))
                    {
                        //TODO: Add functionality for Quincarrier, Enhanced cards etc.

                        _discards.Add(card);

                        foreach (Resource r in card.Resources)
                        {
                            if (resourceToCheck == Resource.Wild || r == resourceToCheck || r == Resource.Wild)
                            {
                                _resources.Add(r);

                                if (amount != 0 && _resources.Count == amount)
                                    finishedSelecting = true;
                            }

                        }
                    }
                }    
            }

            yield return null;
        }

        foreach (PlayerCard c in _discards.Cast<PlayerCard>())
            player.Hand.Remove(c);

        player.Deck.Discard(Discards);

        UIManager.InStateMachine = false;
        FinishButton.ToggleFinishButton(false, FinishedSelecting);
    }

    public void FinishedSelecting()
    {
        finishedSelecting = true;
        FinishButton.ToggleFinishButton(false, FinishedSelecting);
    }

    #region Properties
    public StateMachine StateMachine
    {
        get { return stateMachine; }
    }
    public List<IState> States
    {
        get { return states; }
    }
    public List<Card> Discards
    {
        get { return _discards; }
    }
    public List<Resource> Resources
    {
        get { return _resources; }
    }
    #endregion

    #region States
    abstract class BasePayCostSystemState : BaseState
    {
        protected PayCostSystem owner = instance;
    }

    class GetCandidatesState : BasePayCostSystemState
    {
        public override void Enter()
        {
            owner.Clear();
        }

        public override void Execute()
        {
            owner.candidates.AddRange(owner.zone);

            /*List<Card> supports = new List<Card>();

            foreach (PlayerCard card in GameObject.Find("UpgradesAndSupports").transform.GetComponentsInChildren<PlayerCard>())
            {
                supports.Add(card);
            }

            foreach (Card c in supports)
            {
                if the card provides a resource, add it to candidates [Quincarrier, the Enhanced cards, Eye of Agamotto)
            }*/

            owner.ChangeState(2);
        }
    }

    class SelectTargetsState : BasePayCostSystemState
    {
        public override void Enter()
        {
            owner.StartCoroutine(SelectTargets());
        }

        IEnumerator SelectTargets()
        {
            while (owner.Resources.Count < owner.cardToPlay.CardCost)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    owner.pointerEventData = new PointerEventData(owner.eventSystem)
                    {
                        position = Input.mousePosition,
                    };

                    List<RaycastResult> results = new();

                    owner.raycaster.Raycast(owner.pointerEventData, results);

                    results.RemoveAll(x => x.gameObject.GetComponent<PlayerCard>() == null || x.gameObject.GetComponent<PlayerCard>() == owner.cardToPlay);
                    var card = results.Count > 0 ? results[0].gameObject.GetComponent<PlayerCard>() : null;
                            
                    if (owner.candidates.Contains(card))
                    {
                        /*if (card.cardType is UpgradeType || card.cardType is SupportType)
                        {
                            if (!card.Exhausted)
                            {
                                card.exhausted = true;
                                //resources.Add(card.data.actionResource);
                            }
                        }
                        */
                        
                        if (!owner.Discards.Contains(card))
                        {
                            owner.Discards.Add(card);
                            owner.Resources.AddRange(card.Resources);
                        }
                        else
                        {
                            owner.Discards.Remove(card);
                            for (int i = owner.Resources.Count -1; owner.Resources.Count != owner.Resources.Count - card.Resources.Count; i--)
                            {
                                owner.Resources.RemoveAt(i);
                            }
                        }
                    }
                }

                yield return null;
            }
            
            owner.ChangeState(0);
        }
    }
    #endregion
}
