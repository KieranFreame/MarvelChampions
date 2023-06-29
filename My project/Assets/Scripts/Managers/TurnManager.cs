using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public static List<Player> Players { get; private set; } = new();
    public Player FirstPlayer { get; set; }
    public int FirstPlayerIndex { get; set; } = 0;
    public Player CurrPlayer { get; set; }

    public static event UnityAction OnStartPlayerPhase;
    public static event UnityAction OnEndPlayerPhase;
    public static event UnityAction OnStartVillainPhase;
    public static event UnityAction OnEndVillainPhase;

    public static bool IsPlayerPhase { get; private set; } = true;
    private void Start()
    {
        Players.AddRange(FindObjectsOfType<Player>());
        CurrPlayer = FirstPlayer = Players[FirstPlayerIndex];

        ScenarioManager.inst.GenerateDeck();
    }

    public void ChangeFirstPlayer()
    {
        FirstPlayerIndex++;

        if (FirstPlayerIndex >= Players.Count)
            FirstPlayerIndex = 0;

        FirstPlayer = Players[FirstPlayerIndex];
    }

    public void EndPlayerPhase()
    {
        IsPlayerPhase = false;
        OnEndPlayerPhase?.Invoke();
        OnStartVillainPhase?.Invoke();
    }

    public void EndVillainPhase()
    {
        IsPlayerPhase = true;
        OnEndVillainPhase?.Invoke();
        OnStartPlayerPhase?.Invoke();
    }
}
