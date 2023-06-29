using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Threading.Tasks;

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

    #region Fields
    //readonly List<PlayerCard> candidates = new();
    private readonly List<ICard> _discards = new();
    private readonly List<Resource> _resources = new();
    //PlayerCard cardToPlay;
    //List<PlayerCard> zone;

    private bool finishedSelecting = false;
    #endregion

    private void Clear()
    {
        Resources.Clear();
        _discards.Clear();
    }

    public async Task PayForCard(PlayerCard cardToPlay)
    {
        UIManager.MakingSelection = true;
        PlayerCard card = default;
        int resourceCount = 0;
        Clear();

        while (resourceCount < cardToPlay.CardCost)
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

                if (card != default && card != cardToPlay) 
                {
                    if (!_discards.Contains(card))
                    {
                        _discards.Add(card);

                        if (card.Data.cardType == CardType.Resource)
                            resourceCount += (card as ResourceCard).GetResources().Count;
                        else
                            resourceCount += card.Resources.Count;
                    }
                    else
                    {
                        _discards.Remove(card);

                        if (card.Data.cardType == CardType.Resource)
                            resourceCount -= (card as ResourceCard).GetResources().Count;
                        else
                            resourceCount -= card.Resources.Count;
                    }
                }
            }

            await Task.Yield();
        }

        for (int i = _discards.Count - 1; i >= 0; i--)
        {
            switch ((_discards[i] as PlayerCard).Data.cardType)
            {
                case CardType.Resource:
                    _resources.AddRange((_discards[i] as ResourceCard).GetResources());

                    if ((_discards[i] as PlayerCard).Effect != null)
                        await ((_discards[i] as ResourceCard).Effect as ResourceCardEffect).WhenSpent();

                    cardToPlay.Owner.Hand.Remove((_discards[i] as PlayerCard));
                    continue;
                case CardType.Upgrade:
                case CardType.Support:
                    if ((_discards[i] as PlayerCard).InPlay)
                    {
                        await (_discards[i] as PlayerCard).Effect.Activate();
                        _discards.RemoveAt(i);
                    }
                    else
                    {
                        _resources.AddRange((_discards[i] as PlayerCard).Resources);
                        cardToPlay.Owner.Hand.Remove((_discards[i] as PlayerCard));
                    }
                    continue;
                case CardType.Ally:
                case CardType.Event:
                    _resources.AddRange((_discards[i] as PlayerCard).Resources);
                    cardToPlay.Owner.Hand.Remove((_discards[i] as PlayerCard));
                    continue;
            } 
        }

        cardToPlay.Owner.Deck.Discard(_discards);
        UIManager.MakingSelection = false;
    }

    public async Task GetResources(Resource resourceToCheck = Resource.Any, int amount = 0)
    {
        UIManager.MakingSelection = true;
        FinishButton.ToggleFinishButton(true, FinishedSelecting);
        PlayerCard card = null;
        Player player = FindObjectOfType<Player>();
        finishedSelecting = false;
        Clear();

        int resourceCount = 0;

        while (!finishedSelecting || resourceCount >= amount)
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
                    if (resourceToCheck != Resource.Any)
                        if (!card.Resources.Contains(resourceToCheck) && !card.Resources.Contains(Resource.Wild))
                            continue;

                    //TODO: Add functionality for Quincarrier, Enhanced cards etc.

                    if (!_discards.Contains(card))
                    {
                        _discards.Add(card);
                        resourceCount += card.Resources.Count;
                    }
                    else
                    {
                        _discards.Remove(card);
                        resourceCount -= card.Resources.Count;
                    }
                    

                    if (resourceCount >= amount)
                        break;
                }    
            }

            await Task.Yield();
        }

        foreach (PlayerCard c in _discards.Cast<PlayerCard>())
        {
            _resources.AddRange(c.Resources);
            player.Hand.Remove(c);
        }

        player.Deck.Discard(_discards);

        UIManager.MakingSelection = false;
        FinishButton.ToggleFinishButton(false, FinishedSelecting);
    }

    public void FinishedSelecting()
    {
        finishedSelecting = true;
        FinishButton.ToggleFinishButton(false, FinishedSelecting);
    }

    #region Properties
    public List<Resource> Resources
    {
        get { return _resources; }
    }
    #endregion
}
