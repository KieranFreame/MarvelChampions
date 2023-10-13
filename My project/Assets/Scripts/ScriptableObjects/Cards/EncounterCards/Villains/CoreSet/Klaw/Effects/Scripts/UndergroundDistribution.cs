using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Underground Distribution", menuName = "MarvelChampions/Card Effects/Klaw/Underground Distribution")]
public class UndergroundDistribution : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        EncounterCardData data = ScenarioManager.inst.EncounterDeck.Search("Defense Network") as EncounterCardData;
        SchemeCard network = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("SideSchemeTransform").transform) as SchemeCard;

        ScenarioManager.sideSchemes.Add(network);
        await network.OnRevealCard();

        do
        {
            data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
            ScenarioManager.inst.EncounterDeck.Mill(1);
        } while (data is not MinionCardData);

        ScenarioManager.inst.EncounterDeck.discardPile.Remove(data);
        ScenarioManager.inst.EncounterDeck.limbo.Add(data);

        MinionCard minion = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("MinionTransform").transform) as MinionCard;
        VillainTurnController.instance.MinionsInPlay.Add(minion);
        await minion.Effect.OnEnterPlay(owner, minion, player);
    }
}
