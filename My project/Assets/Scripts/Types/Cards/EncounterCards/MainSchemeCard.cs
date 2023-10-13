using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

public class MainSchemeCard : SchemeCard
{
    public delegate Task StepOne();
    public List<StepOne> AfterStepOne = new();

    public override void LoadCardData(EncounterCardData data, Villain owner)
    {
        Data = data;

        _acceleration = (data as SchemeCardData).Acceleration;

        Threat = new(this, (data as SchemeCardData).StartingThreat, (data as SchemeCardData).Acceleration, (data as SchemeCardData).MaximumThreat);

        ScenarioManager.inst.EncounterDeck.OnDeckReset += IncreaseAcceleration;
        //Threat.WhenCompleted += WhenCompleted;

        base.LoadCardData(data, owner);
    }

    public override async void WhenDefeated()
    {
        await Effect.WhenDefeated();
    }

    //public async void WhenCompleted() => await Effect.WhenCompleted();

    public async Task Accelerate()
    {
        Debug.Log(name + ": Accelerating by " + _acceleration + " threat.");
        Threat.GainThreat(_acceleration);
        
        for (int i = AfterStepOne.Count- 1; i >= 0; i--)
        {
            await AfterStepOne[i]();
        }
    }

    private void IncreaseAcceleration()
    {
        _acceleration++;
        Threat.Acceleration = _acceleration * TurnManager.Players.Count;
    }

    private int _acceleration;
    public int Acceleration { get => _acceleration; }
    public int MaximumThreat { get => (Data as SchemeCardData).MaximumThreat; }
}
