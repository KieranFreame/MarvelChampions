using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchemeCard : EncounterCard
{
    public int StartingThreat { get => (Data as SchemeCardData).StartingThreat; }
    public Threat Threat { get; protected set; }

    public override void LoadCardData(EncounterCardData data, Villain owner)
    {
        if (this is not MainSchemeCard)
            Threat = new Threat(this, (data as SchemeCardData).StartingThreat);

        base.LoadCardData(data, owner);
    }

    public virtual async void WhenDefeated()
    {
        await Effect.WhenDefeated();
        ScenarioManager.sideSchemes.Remove(this);
        ScenarioManager.inst.EncounterDeck.Discard(this);
    }
}
