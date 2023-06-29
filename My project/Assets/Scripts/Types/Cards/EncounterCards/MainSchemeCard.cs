using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

public class MainSchemeCard : SchemeCard
{
    public List<IStepOne> AfterStepOne = new();

    public override void LoadCardData(EncounterCardData data, Villain owner)
    {
        Data = data;
        Threat threat = GetComponent<Threat>();
        threat.SetThreat((data as SchemeCardData).StartingThreat, (data as SchemeCardData).Acceleration, (data as SchemeCardData).MaximumThreat);
        base.LoadCardData(data, owner);
    }

    private void Start()
    {
        GetComponent<Threat>().WhenCompleted += WhenCompleted;
        //LoadCardData(temp_data, FindObjectOfType<Villain>());
    }

    protected override void WhenDefeated()
    {
        return;
    }

    private void WhenCompleted() { Debug.Log("Main Scheme has reached Maximum Threat! You Lose!"); }

    public async Task Accelerate()
    {
        Debug.Log(name + ": Accelerating by " + Acceleration + " threat.");
        GetComponent<Threat>().GainThreat(Acceleration);
        
        foreach (IStepOne subscriber in AfterStepOne)
        {
            await subscriber.Execute();
        }
    }

    public int Acceleration { get => (Data as SchemeCardData).Acceleration; }
    public int MaximumThreat { get => (Data as SchemeCardData).MaximumThreat; }
}
