using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Full Auto", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/Full Auto")]
public class FullAuto : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is AlterEgo)
            ScenarioManager.inst.Surge(player);
        else //hero
        {
            int damage = 0;

            for (int i = owner.CharStats.Attacker.CurrentAttack; i > 0; i--)
            {
                damage += (ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData).boostIcons;
                ScenarioManager.inst.EncounterDeck.Mill(1);
            }

            List<ICharacter> targets = new() { TurnManager.instance.CurrPlayer };
            targets.AddRange((targets[0] as Player).CardsInPlay.Allies);

            await IndirectDamageHandler.inst.HandleIndirectDamage(targets, damage);
        }
    }
}
