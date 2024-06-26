using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Yon-Rogg's Treason", menuName = "MarvelChampions/Card Effects/Nemesis/Captain Marvel/Yon-Rogg's Treason")]
public class YonRoggsTreason : EncounterCardEffect
{
    public override Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;
        var discards = new List<PlayerCard>(player.Hand.cards.Where(x => x.Resources.Contains(Resource.Energy)));

        if (discards.Count > 0)
        {
            foreach (PlayerCard c in discards)
            {
                player.Hand.Discard(c);
            }
        }
        else
        {
            ScenarioManager.inst.Surge(player);
        }

        return Task.CompletedTask;
    }
}
