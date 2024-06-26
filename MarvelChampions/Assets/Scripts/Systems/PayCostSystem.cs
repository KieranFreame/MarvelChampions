using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Events;

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
    public Dictionary<PlayerCard, List<Resource>> availableResources = new();
    public readonly List<PlayerCard> _selected = new();
    private readonly List<Resource> _resources = new();
    bool playerSelected = false;
    bool finished;
    #endregion

    #region Events
    public UnityAction GetAvailableResources;
    #endregion

    private void Clear()
    {
        availableResources.Clear();
        _resources.Clear();
        _selected.Clear();
    }

    public async Task PayForCard(PlayerCard cardToPlay)
    {
        Clear();
        GetAvailableResources?.Invoke();
        availableResources.Remove(cardToPlay);

        int cost = cardToPlay.CardCost;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                pointerEventData = new PointerEventData(eventSystem)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new();
                raycaster.Raycast(pointerEventData, results);
                var obj = results.Where(x => x.gameObject.GetComponent<PlayerCard>() != null).ToList();

                if (obj.Count != 0 && obj[0].gameObject.TryGetComponent(out PlayerCard card)) //PlayerCard selected
                {
                    if (availableResources.ContainsKey(card))
                    {
                        if (!_selected.Contains(card))
                        {
                            cost = (cost - availableResources[card].Count < 0) ? 0 : cost - availableResources[card].Count;
                            _selected.Add(card);
                        }
                        else
                        {
                            cost += (cost + availableResources[card].Count > cardToPlay.CardCost) ? cardToPlay.CardCost : cost + availableResources[card].Count;
                            _selected.Remove(card);
                        }
                    }
                }
                else
                {
                    obj = results.Where(x => x.gameObject.GetComponent<Player>()).ToList();

                    if (obj.Count > 0 && obj[0].gameObject.TryGetComponent(out Player p) && p.Identity.ActiveEffect is IGenerate effect) //Identity selected (Peter Parker, Venom)
                    {
                        if (!playerSelected)
                            cost = (cost - effect.GetResources().Count < 0) ? 0 : cost - effect.GetResources().Count;
                        else
                            cost = (cost + effect.GetResources().Count > cardToPlay.CardCost) ? cardToPlay.CardCost : cost + effect.GetResources().Count;

                        playerSelected = !playerSelected;

                        if (playerSelected) Debug.Log("Added Resource from " + p.Name);
                    }
                }
            }

            if (cost <= 0)
            {
                HandleResources();
                break;
            }

            await Task.Yield();
        }
    }

    public async Task GetResources(Dictionary<Resource, int> needed)
    {
        Clear();
        GetAvailableResources?.Invoke();
        int wildCount = 0;
        finished = false;

        while (!finished)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                List<RaycastResult> results = new();
                raycaster.Raycast(pointerEventData, results);
                var obj = results.Where(x => x.gameObject.GetComponent<PlayerCard>() != null).ToList();

                if (obj.Count != 0 && obj[0].gameObject.TryGetComponent(out PlayerCard card)) //PlayerCard selected
                {
                    if (availableResources.ContainsKey(card))
                    {
                        if (!_selected.Contains(card))
                        {
                            foreach (var r in availableResources[card])
                            {
                                if (needed.ContainsKey(r))
                                    needed[r]--;
                                else if (r == Resource.Wild)
                                    wildCount++;
                            }
                        }
                        else
                        {
                            foreach (var r in availableResources[card])
                            {
                                if (needed.ContainsKey(r))
                                    needed[r]++;
                                else if (r == Resource.Wild)
                                    wildCount--;
                            }
                        }
                    }
                }
                else
                {
                    obj = results.Where(x => x.gameObject.GetComponent<Player>()).ToList();

                    if (obj.Count > 0 && obj[0].gameObject.TryGetComponent(out Player p) && p.Identity.ActiveEffect is IGenerate effect) //Identity selected (Peter Parker, Venom)
                    {
                        if (!playerSelected)
                        {
                            foreach (var r in effect.GetResources())
                            {
                                if (needed.ContainsKey(r))
                                    needed[r]++;
                                else if (r == Resource.Wild)
                                    wildCount--;
                            }
                        }
                        else
                        {
                            foreach (var r in effect.GetResources())
                            {
                                if (needed.ContainsKey(r))
                                    needed[r]++;
                                else if (r == Resource.Wild)
                                    wildCount--;
                            }
                        }

                        playerSelected = !playerSelected;

                        if (playerSelected) Debug.Log("Added Resource from " + p.Name);
                    }
                }
            }

            int remainder = 0;

            foreach (var r in needed)
            {
                if (r.Value > 0)
                    remainder += r.Value;
            }

            if (wildCount >= remainder)
            {
                HandleResources(); 
                break;
            }

            await Task.Yield();
        }
    }

    public void FinishedSelecting()
    {
        finished = true;
        FinishButton.ToggleFinishButton(false, FinishedSelecting);
    }

    private void HandleResources()
    {
        var player = TurnManager.instance.CurrPlayer;

        foreach (var card in _selected)
        {
            if (card.gameObject.name == player.name)
            {
                _resources.AddRange(((IGenerate)player.Identity.ActiveEffect).GenerateResource());
                continue;
            }

            if (card.InPlay)
            {
                var eff = (IGenerate)card.Effect;
                _resources.AddRange(eff.GenerateResource());
            }
            else
            {
                _resources.AddRange(card.Resources);
                player.Hand.Discard(card);
            }
        }
    }

    #region Properties
    public List<Resource> Resources
    {
        get { return _resources; }
    }
    #endregion
}
