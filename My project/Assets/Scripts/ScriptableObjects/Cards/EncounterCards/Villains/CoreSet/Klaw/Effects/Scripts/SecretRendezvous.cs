using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Secret Rendezvous", menuName = "MarvelChampions/Card Effects/Klaw/Secret Rendezvous")]
public class SecretRendezvous : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        EncounterCardData data;

        do
        {
            data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
            ScenarioManager.inst.EncounterDeck.Mill(1);
        } while (data is not MinionCardData);

        ScenarioManager.inst.EncounterDeck.discardPile.Remove(data);
        ScenarioManager.inst.EncounterDeck.limbo.Add(data);

        MinionCard minion = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("MinionTransform").transform) as MinionCard;
        VillainTurnController.instance.MinionsInPlay.Add(minion);



        await Task.Yield();
    }
}
