using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Underground Distribution", menuName = "MarvelChampions/Card Effects/Klaw/Underground Distribution")]
public class UndergroundDistribution : EncounterCardEffect
{
    public override async Task OnEnterPlay()
    {
        EncounterCardData data = ScenarioManager.inst.EncounterDeck.Search("Defense Network") as EncounterCardData;
        SchemeCard network = (SchemeCard)CreateCardFactory.Instance.CreateCard(data, GameObject.Find("SideSchemeTransform").transform);

        ScenarioManager.sideSchemes.Add(network);
        await network.OnRevealCard();

        do
        {
            data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
            ScenarioManager.inst.EncounterDeck.Mill(1);
        } while (data is not MinionCardData);

        ScenarioManager.inst.EncounterDeck.discardPile.Remove(data);
        ScenarioManager.inst.EncounterDeck.limbo.Add(data);

        MinionCard minion = (MinionCard)CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform);
        VillainTurnController.instance.MinionsInPlay.Add(minion);
        await minion.Effect.OnEnterPlay();
    }
}
