using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

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
    public readonly List<ICard> _discards = new();
    private readonly List<IGenerate> _effects = new();
    private readonly List<Resource> _resources = new();
    PlayerCard _cardToPlay;
    //List<PlayerCard> zone;
    #endregion

    private void Clear()
    {
        Resources.Clear();
        _discards.Clear();
        _effects.Clear();
    }

    public async Task PayForCard(PlayerCard cardToPlay)
    {
        UIManager.MakingSelection = true;
        int resourceCount = 0;
        Clear();

        _cardToPlay = cardToPlay;

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

                foreach (RaycastResult r in results)
                {
                    if (r.gameObject.GetComponent<PlayerCard>() != null)
                    {
                        PlayerCard card = r.gameObject.GetComponent<PlayerCard>();

                        if (card != cardToPlay)
                        {
                            resourceCount += HandleCardSelected(card);
                            continue;
                        }
                    }
                    else if (r.gameObject.GetComponent<Player>() != null)
                    {
                        Player p = r.gameObject.GetComponent<Player>();

                        if (p.Identity.ActiveEffect is IGenerate)
                        {
                            IGenerate eff = p.Identity.ActiveEffect as IGenerate;

                            if (!_effects.Contains(eff))
                            {
                                if (eff.CanGenerateResource() > 0)
                                {
                                    _effects.Add(eff);
                                    resourceCount++;
                                }
                            }
                            else
                            {
                                _effects.Remove(eff);
                                resourceCount--;
                            }    
                        }
                    }
                }
            }

            await Task.Yield();
        }

        foreach (var eff in _effects)
        {
            _resources.AddRange(eff.GenerateResource());
        }

        foreach (PlayerCard card in _discards.Cast<PlayerCard>())
        {
            if (card.Data.cardType is CardType.Resource)
            {
                _resources.AddRange((card as ResourceCard).GetResources());

                if (card.Effect != null)
                    await (card.Effect as ResourceCardEffect).WhenSpent();
            }
            else
            {
                _resources.AddRange(card.Resources);
            }

            cardToPlay.Owner.Hand.Remove(card);
        }

        cardToPlay.Owner.Deck.Discard(_discards);
        UIManager.MakingSelection = false;
    }
    public async Task<bool> GetResources(Resource resourceToCheck = Resource.Any, int amount = 0, bool enableFinish = false)
    {
        UIManager.MakingSelection = true;
        CancellationToken token = enableFinish ? FinishButton.ToggleFinishButton(true, FinishedSelecting) : default;
        Player player = FindObjectOfType<Player>();
        Clear();

        int resourceCount = 0;

        Debug.Log($"Spend {amount} {resourceToCheck} resources");

        while (!token.IsCancellationRequested && resourceCount < amount)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                pointerEventData = new PointerEventData(eventSystem)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new();

                raycaster.Raycast(pointerEventData, results);

                foreach (RaycastResult r in results)
                {
                    if (r.gameObject.GetComponent<PlayerCard>() != null)
                    {
                        PlayerCard card = r.gameObject.GetComponent<PlayerCard>();

                        if (card.Resources.Contains(resourceToCheck) || card.Resources.Contains(Resource.Wild) || resourceToCheck == Resource.Any || card.Effect is IGenerate)
                        {
                            resourceCount += HandleCardSelected(card);
                            continue;
                        }
                        
                    }
                    else if (r.gameObject.GetComponent<Player>() != null)
                    {
                        Player p = r.gameObject.GetComponent<Player>();

                        if (p.Identity.ActiveEffect is IGenerate)
                        {
                            IGenerate eff = p.Identity.ActiveEffect as IGenerate;

                            if (!_effects.Contains(eff))
                            {
                                if (eff.CompareResource(resourceToCheck))
                                {
                                    _effects.Add(eff);
                                    resourceCount++;
                                }
                            }
                            else
                            {
                                _effects.Remove(eff);
                                resourceCount--;
                            }
                        }
                    }
                }
            }

            await Task.Yield();
        }

        if (resourceCount > 0)
        {
            foreach (var eff in _effects)
            {
                _resources.AddRange(eff.GenerateResource());
            }

            foreach (PlayerCard card in _discards.Cast<PlayerCard>())
            {
                if (card.Data.cardType is CardType.Resource)
                {
                    _resources.AddRange((card as ResourceCard).GetResources());

                    if (card.Effect != null)
                        await (card.Effect as ResourceCardEffect).WhenSpent();
                }
                else
                {
                    _resources.AddRange(card.Resources);
                }

                player.Hand.Remove(card);
            }

            player.Deck.Discard(_discards);
        }

        
        UIManager.MakingSelection = false;

        if (token != default)
            FinishButton.ToggleFinishButton(false, FinishedSelecting);

        return resourceCount > 0;
    }

    private int HandleCardSelected(PlayerCard card)
    {
        if (!card.InPlay)
        {
            if (!_discards.Contains(card))
            {
                _discards.Add(card);
                if (card.Data.cardType == CardType.Resource)
                    return (card as ResourceCard).GetResources().Count;
                else
                    return card.Resources.Count;
            }
            else
            {
                _discards.Remove(card);
                if (card.Data.cardType == CardType.Resource)
                    return -(card as ResourceCard).GetResources().Count;
                else
                    return -card.Resources.Count;
            }    
        }
        else
        {
            if (card.Effect is IGenerate)
            {
                IGenerate eff = card.Effect as IGenerate;
                if (!_effects.Contains(eff))
                {
                    if (eff.CanGenerateResource() > 0)
                    {
                        _effects.Add(eff);
                        return eff.ResourceCount();
                    }
                }
                else
                {
                    _effects.Remove(eff);
                    return -eff.ResourceCount();
                }
            }        
        }

        return 0;
    }

    public void FinishedSelecting()
    {
        FinishButton.ToggleFinishButton(false, FinishedSelecting);
    }

    #region Properties
    public List<Resource> Resources
    {
        get { return _resources; }
    }
    #endregion
}
