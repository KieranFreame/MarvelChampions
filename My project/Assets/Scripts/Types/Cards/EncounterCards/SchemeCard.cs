using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchemeCard : EncounterCard
{
    public int StartingThreat { get => (Data as SchemeCardData).StartingThreat; }

    private void OnEnable()
    {
        GetComponent<Threat>().WhenDefeated += WhenDefeated;
    }

    private void OnDisable()
    {
        GetComponent<Threat>().WhenDefeated -= WhenDefeated;
    }

    public override void LoadCardData(EncounterCardData data, Villain owner)
    {
        if (this is not MainSchemeCard)
            GetComponent<Threat>().SetThreat((data as SchemeCardData).StartingThreat);

        base.LoadCardData(data, owner);
    }

    protected override void WhenDefeated()
    {
        ScenarioManager.sideSchemes.Remove(this);
        base.WhenDefeated();
    }
}
