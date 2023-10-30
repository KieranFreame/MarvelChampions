using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Yon-Rogg's Treason", menuName = "MarvelChampions/Card Effects/Nemesis/Captain Marvel/Yon-Rogg's Treason")]
public class YonRoggsTreason : EncounterCardEffect
{
    List<PlayerCard> discards = new();

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        foreach (PlayerCard c in player.Hand.cards)
        {
            if (c.Resources.Contains(Resource.Energy))
            {
                discards.Add(c);
            }
        }

        if (discards.Count > 0)
        {
            foreach (PlayerCard c in discards)
            {
                player.Hand.Remove(c);
                player.Deck.Discard(c);
            }
        }
        else
        {
            ScenarioManager.inst.Surge(player);
        }

        await Task.Yield();
    }
}
