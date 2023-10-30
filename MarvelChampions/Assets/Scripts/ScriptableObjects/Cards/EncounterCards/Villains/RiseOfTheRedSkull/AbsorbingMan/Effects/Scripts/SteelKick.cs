using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Steel Kick", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Steel Kick")]
public class SteelKick : EncounterCardEffect 
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;

        if (player.Identity.ActiveIdentity is AlterEgo)
        {
            ScenarioManager.inst.MainScheme.Threat.GainThreat(_owner.VillainTraits.Contains("Metal") ? 3 : 2);
        }
        else
        {
            List<ICharacter> targets = new() { TurnManager.instance.CurrPlayer };
            targets.AddRange((targets[0] as Player).CardsInPlay.Allies);

            await IndirectDamageHandler.inst.HandleIndirectDamage(targets, _owner.VillainTraits.Contains("Metal") ? 4 : 3);
        }
    }
}
