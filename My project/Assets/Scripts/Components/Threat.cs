using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Threat : MonoBehaviour
{
    SchemeCard owner;
    public int StartingThreat { get; set; }
    public int CurrentThreat { get; set; }

    //Main Scheme
    private int maxThreat;
    private int acceleration;
    public int Acceleration {
        get => acceleration;
        set
        {
            acceleration = value;
            AccelerationChanged?.Invoke(acceleration);
        }
    } 
    public int MaxThreat
    {
        get => maxThreat;
        set
        {
            maxThreat = value;
            MaxThreatChanged?.Invoke(maxThreat);
        }
    }
        

    public event UnityAction WhenDefeated;
    public event UnityAction<int> ThreatChanged;
    public event UnityAction<int> MaxThreatChanged;
    public event UnityAction<int> AccelerationChanged;
    public event UnityAction WhenCompleted;

    private void OnEnable()
    {
        owner = GetComponent<SchemeCard>();
    }

    public void RemoveThreat(int thwart)
    {
        if (thwart > 0)
        {
            CurrentThreat -= thwart;

            if (CurrentThreat <= 0)
            {
                if (owner is not MainSchemeCard)
                    WhenDefeated?.Invoke();
                else
                    CurrentThreat = 0;
            }
        }

        ThreatChanged?.Invoke(CurrentThreat);
    }

    public void GainThreat(int threat)
    {
        CurrentThreat += threat;

        if (owner is MainSchemeCard)
        {
            if (CurrentThreat >= MaxThreat)
                WhenCompleted?.Invoke();
        }

        ThreatChanged?.Invoke(CurrentThreat);
    }

    public void SetThreat(int startThreat)
    {
        CurrentThreat = StartingThreat = startThreat;
    }
    
    public void SetThreat(int startThreat, int acceleration, int maxThreat) //MainScheme
    {
        CurrentThreat = StartingThreat = startThreat;
        Acceleration = acceleration * TurnManager.Players.Count;
        MaxThreat = maxThreat * TurnManager.Players.Count;

        MaxThreatChanged?.Invoke(MaxThreat);
    }
}
