using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Monster")]
public class Monster : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (player.CharStats.Attacker.Stunned)
        {
            player.CharStats.Health.TakeDamage(new(player, 2, card: card, owner:owner));
        }
        else
        {
            player.CharStats.Attacker.Stunned = true;
        }

        return Task.CompletedTask;
    }

    #region Boost
    public override Task Resolve()
    {
        EncounterCardData monster = ScenarioManager.inst.EncounterDeck.discardPile.LastOrDefault(x => x.cardName == "Monster") as EncounterCardData;

        if (monster != null)
        {
            ScenarioManager.inst.EncounterDeck.discardPile.Remove(monster);
            ScenarioManager.inst.EncounterDeck.AddToDeck(monster);
        }

        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        EffectResolutionManager.Instance.ResolvingEffects.Push(this);
        return Task.CompletedTask;
    }
    #endregion
}
