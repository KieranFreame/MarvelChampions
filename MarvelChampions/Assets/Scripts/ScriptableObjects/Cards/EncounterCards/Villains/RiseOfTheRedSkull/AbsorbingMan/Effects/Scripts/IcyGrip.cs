using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Icy Grip", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Icy Grip")]
public class IcyGrip : EncounterCardEffect 
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        player.CharStats.Attacker.Stunned = true;

        if (owner.VillainTraits.Contains("Ice"))
        {
            List<ICharacter> targets = new() { TurnManager.instance.CurrPlayer };
            targets.AddRange((targets[0] as Player).CardsInPlay.Allies);

            await IndirectDamageHandler.inst.HandleIndirectDamage(targets, 2);
        }
    }

    public override Task Boost(Action action)
    {
        if (ScenarioManager.inst.ActiveVillain.VillainTraits.Contains("Ice") || ScenarioManager.inst.ActiveVillain.VillainTraits.Contains("Metal"))
        {
            ScenarioManager.inst.ActiveVillain.CharStats.Health.Tough = true;
        }

        return Task.CompletedTask;
    }
}
