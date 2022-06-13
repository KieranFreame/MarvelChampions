using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

        states.Add(new IdleState(this));
        states.Add(new ClearState(this));
        states.Add(new GetCandidatesState(this));
        states.Add(new SelectTargetsState(this));

        stateMachine.ChangeState(states[0]);
    }
    #endregion

    #region Pointer
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    void Start()
    {
        raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }
    #endregion

    public StateMachine stateMachine = new StateMachine();
    public List<IState> states = new List<IState>();

    List<dynamic> candidates = new List<dynamic>();
    public List<Card> discards;
    public List<Resource> resources;
    CardData cardToPlay;
    List<Card> zone;

    private void Update()
    {
        stateMachine.Update();
    }

    public IEnumerator GetTargets(CardData data, List<Card> targets)
    {
        this.cardToPlay = data;
        this.zone = targets;
        stateMachine.ChangeState(states[1]);

        while (stateMachine.currentState != states[0])
            yield return null;
    }

    #region States
    abstract class BasePayCostSystemState : BaseState
    {
        protected PayCostSystem owner;
    }

    class IdleState : BasePayCostSystemState
    {
        public IdleState(PayCostSystem owner)
        {
            this.owner = owner;
        }
    }

    class ClearState : BasePayCostSystemState
    {
        public ClearState(PayCostSystem owner)
        {
            this.owner = owner;
        }

        public override void Enter()
        {
            if (owner.discards.Count > 0)
                owner.discards.Clear();
            
            if (owner.resources.Count > 0)
                owner.resources.Clear();

            owner.stateMachine.ChangeState(owner.states[2]);
        }
    }

    class GetCandidatesState : BasePayCostSystemState
    {
        public GetCandidatesState(PayCostSystem owner)
        {
            this.owner = owner;
        }

        public override void Execute()
        {
            owner.candidates.AddRange(owner.zone);

            /*List<Card> supports = new List<Card>();

            foreach (CardUI ui in GameObject.Find("UpgradesAndSupports").transform.GetComponentsInChildren<CardUI>())
            {
                supports.Add(ui.card);
            }

            foreach (Card c in supports)
            {
                if the card provides a resource, add it to candidates [Quincarrier, the Enhanced cards, Eye of Agamotto)
            }*/

            owner.stateMachine.ChangeState(owner.states[3]);
        }
    }

    class SelectTargetsState : BasePayCostSystemState
    {
        public SelectTargetsState(PayCostSystem owner)
        {
            this.owner = owner;
        }

        public override void Execute()
        {
            if (Input.GetMouseButton(0))
            {
                owner.pointerEventData = new PointerEventData(owner.eventSystem);
                owner.pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();

                owner.raycaster.Raycast(owner.pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.transform.parent.GetComponent<CardUI>() != null)
                    {
                        var card = result.gameObject.transform.parent.GetComponent<CardUI>().card;

                        foreach (dynamic c in owner.candidates)
                        {
                            if (c.data == card.data)
                            {
                                /*if (card.data.cardType is UpgradeType || card.data.cardType is SupportType)
                                {
                                    if (!card.exhausted)
                                    {
                                        card.exhausted = true;
                                        //resources.Add(card.data.actionResource);
                                    }
                                }
                                else //card is in hand
                                {*/
                                    if (!owner.discards.Contains(card))
                                    {
                                        owner.discards.Add(card);
                                        owner.resources.AddRange(card.GetResource(owner.cardToPlay as PlayerCard));
                                    }
                                //} 

                                if (owner.resources.Count >= (owner.cardToPlay as PlayerCard).cardCost)
                                    owner.stateMachine.ChangeState(owner.states[0]);
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
}
