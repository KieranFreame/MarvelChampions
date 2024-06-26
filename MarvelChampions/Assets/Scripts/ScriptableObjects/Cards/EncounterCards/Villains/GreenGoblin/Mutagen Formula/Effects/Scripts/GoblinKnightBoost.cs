using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Knight (Boost)", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Goblin Knight (Boost)")]
public class GoblinKnightBoost : EncounterCardEffect
{
    public override Task Resolve()
    {
        EncounterCardData knight = ScenarioManager.inst.EncounterDeck.discardPile.LastOrDefault(x => x.cardName == "Goblin Knight") as EncounterCardData;

        if (knight != null)
        {
            ScenarioManager.inst.EncounterDeck.discardPile.Remove(knight);
            ScenarioManager.inst.EncounterDeck.AddToDeck(knight);
        }

        return Task.CompletedTask;
    }

    #region Boost
    public override async Task Boost(Action action)
    {
        await EffectManager.Inst.AddEffect(_card, this);
    }
    #endregion
}
