using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons Runner", menuName = "MarvelChampions/Card Effects/Klaw/Weapons Runner")]
public class WeaponsRunner : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.Surge(player);

        await Task.Yield();
    }

    public override Task Boost(Action action)
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Weapons Runner").GetComponent<EncounterCard>();

        card.InPlay = true;
        card.transform.SetParent(GameObject.Find("MinionTransform").transform);
        VillainTurnController.instance.MinionsInPlay.Add(card as MinionCard);

        return Task.CompletedTask;
    }
}
