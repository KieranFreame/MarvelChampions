using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MainSchemeCard : SchemeCard
{
    [SerializeField] SchemeCardData temp_data;

    public delegate IEnumerator StepOne();
    public List<StepOne> AfterStepOne = new();

    public override void LoadCardData(CardData data, GameObject owner)
    {
        GetComponent<Threat>().SetThreat(temp_data.StartingThreat, temp_data.Acceleration, temp_data.MaximumThreat);
        base.LoadCardData(data, owner);
    }

    private void Start()
    {
        GetComponent<Threat>().WhenCompleted += WhenCompleted;
        LoadCardData(temp_data, gameObject);
    }

    protected override void WhenDefeated()
    {
        return;
    }

    private void WhenCompleted() { Debug.Log("Main Scheme has reached Maximum Threat! You Lose!"); }

    public IEnumerator Accelerate()
    {
        Debug.Log(name + ": Accelerating by " + Acceleration + " threat.");
        GetComponent<Threat>().GainThreat(Acceleration);
        
        foreach (StepOne func in AfterStepOne)
        {
            yield return func;
        }
    }

    public int Acceleration { get => temp_data.Acceleration; }
    public int MaximumThreat { get => temp_data.MaximumThreat; }
}
