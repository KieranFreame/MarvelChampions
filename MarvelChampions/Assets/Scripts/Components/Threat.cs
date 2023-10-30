using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Threat //: MonoBehaviour
{
    public SchemeCard Owner { get; private set; }
    public int StartingThreat { get; set; }
    private int currentThreat;
    public int CurrentThreat 
    { 
        get => currentThreat;
        set
        {
            currentThreat = value;
            ThreatChanged?.Invoke(currentThreat);
        } 
    }

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
        

    public event UnityAction WhenDefeated; //make delegate?
    public event UnityAction<int> ThreatChanged;
    public event UnityAction<int> MaxThreatChanged;
    public event UnityAction<int> AccelerationChanged;
    public event UnityAction WhenCompleted;

    public Threat(SchemeCard _owner, int startThreat) //Side Scheme
    {
        Owner = _owner;
        CurrentThreat = StartingThreat = startThreat;
    }
    
    public Threat(SchemeCard _owner, int startThreat, int acceleration, int maxThreat) //Main Scheme
    {
        Owner = _owner;
        CurrentThreat = StartingThreat = startThreat;
        Acceleration = acceleration * TurnManager.Players.Count;
        MaxThreat = maxThreat * TurnManager.Players.Count;
    }

    public void RemoveThreat(int thwart)
    {
        if (thwart > 0)
        {
            CurrentThreat -= thwart;

            if (CurrentThreat <= 0)
            {
                if (Owner is not MainSchemeCard)
                {
                    WhenDefeated?.Invoke();
                    Owner.WhenDefeated();
                }
                else
                    CurrentThreat = 0;
            }
        }
    }

    public void GainThreat(int threat)
    {
        CurrentThreat += threat;

        if (Owner is MainSchemeCard)
        {
            if (CurrentThreat >= MaxThreat)
                WhenCompleted?.Invoke();
        }
    }
}
