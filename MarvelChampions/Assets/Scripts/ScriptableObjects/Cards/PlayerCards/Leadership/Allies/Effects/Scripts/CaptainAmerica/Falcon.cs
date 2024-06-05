using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Falcon", menuName = "MarvelChampions/Card Effects/Leadership/Falcon")]
public class Falcon : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        if (ScenarioManager.sideSchemes.Count == 0 && ScenarioManager.inst.MainScheme.Threat.CurrentThreat == 0) return;

        List<EncounterCardData> cards = ScenarioManager.inst.EncounterDeck.GetTop(3).Cast<EncounterCardData>().ToList();

        int threat = cards.Where(x => x.cardType == CardType.Treachery).Count();

        Debug.Log("Revealed " + threat + " Treacheries from top of the encounter deck");

        await ThwartSystem.Instance.InitiateThwart(new(threat, Card as ICharacter));
    }
}
