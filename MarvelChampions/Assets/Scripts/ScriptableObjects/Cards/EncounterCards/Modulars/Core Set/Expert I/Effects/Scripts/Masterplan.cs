using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Masterplan", menuName = "MarvelChampions/Card Effects/Expert/Masterplan")]
public class Masterplan : EncounterCardEffect
{
    public override async Task Resolve()
    {
        if (ScenarioManager.sideSchemes.Count == 0)
        {
            EncounterCardData data;

            do
            {
                data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
                ScenarioManager.inst.EncounterDeck.Mill(1);
            } while (data is not SchemeCardData);

            ScenarioManager.inst.EncounterDeck.discardPile.Remove(data);
            ScenarioManager.inst.EncounterDeck.limbo.Add(data);

            SchemeCard sideScheme = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("SideSchemeTransform").transform) as SchemeCard;
            ScenarioManager.sideSchemes.Add(sideScheme);

            await sideScheme.OnRevealCard();
        }
        else
        {
            foreach (SchemeCard s in ScenarioManager.sideSchemes)
            {
                s.Threat.GainThreat(4);
            }
        }
    }
}
