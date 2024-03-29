using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Overrun", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Overrun")]
public class Overrun : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration += 2;
        ScenarioManager.inst.MainScheme.Threat.CurrentThreat *= TurnManager.Players.Count;
        return Task.CompletedTask;
    }

    public override async Task WhenDefeated()
    {
        List<EncounterCardData> data = ScenarioManager.inst.EncounterDeck.GetTop(2).Cast<EncounterCardData>().ToList();

        for (int i = 1; i <= 0; i--)
        {
            if (data[i].cardTraits.Contains("Goblin"))
            {
                MinionCard goblin = CreateCardFactory.Instance.CreateCard(data[i], GameObject.Find("MinionTransform").transform) as MinionCard;
                await goblin.Effect.OnEnterPlay(_owner, goblin, TurnManager.instance.CurrPlayer);
                ScenarioManager.inst.EncounterDeck.limbo.Add(data[i]);
            }
            else
            {
                ScenarioManager.inst.EncounterDeck.discardPile.Add(data[i]);
            }
        }

        ScenarioManager.inst.MainScheme.Threat.Acceleration -= 2;
    }
}
