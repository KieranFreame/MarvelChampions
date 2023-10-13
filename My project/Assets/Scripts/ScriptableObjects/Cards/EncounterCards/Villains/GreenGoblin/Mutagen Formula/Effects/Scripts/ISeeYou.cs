using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "I See You", menuName = "MarvelChampions/Card Effects/Mutagen Formula/I See You")]
public class ISeeYou : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is AlterEgo)
            BoostSystem.Instance.BoostCardCount = 0;

        await owner.CharStats.InitiateAttack();
    }

    public override Task Boost(Action action)
    {
        if (VillainTurnController.instance.MinionsInPlay.Any(x => x.CardTraits.Contains("Goblin")))
        {
            action.Value++;
        }

        return Task.CompletedTask;
    }
}
