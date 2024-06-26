using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Harvest", menuName = "MarvelChampions/Card Effects/Nemesis/Ms Marvel/Harvest")]
public class Harvest : EncounterCardEffect
{
    public override Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;
        List<PlayerCard> personas = player.CardsInPlay.Permanents.Where(x => x.CardTraits.Contains("Persona")).ToList();

        if (personas.Count > 0)
        {
            foreach (PlayerCard person in personas)
            {
                person.Exhaust();
            }

            _owner.CharStats.Health.CurrentHealth += personas.Count;
        }
        else
        {
            ScenarioManager.inst.Surge(player);
        }

        return Task.CompletedTask;
    }
}
