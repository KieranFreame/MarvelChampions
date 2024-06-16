using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Mad Genius", menuName = "MarvelChampions/Card Effects/Risky Business/Mad Genius")]
public class MadGenius : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (ScenarioManager.inst.ActiveVillain.Name == "Norman Osborn")
        {
            player.Deck.Mill(RiskyBusiness.Instance.environment.GetCounters());
            return;
        }
        else //Green Goblin
        {
            int minHP = int.MaxValue;
            Player target = null;

            foreach (var p in TurnManager.Players.Where(x => x.Identity.ActiveIdentity is Hero))
            {
                if (p.CharStats.Health.CurrentHealth < minHP)
                {
                    minHP = p.CharStats.Health.CurrentHealth;
                    target = p;
                }
            }

            if (target != null)
            {
                AttackAction attack = await owner.CharStats.Attacker.Attack();

                if (attack != null)
                {
                    attack.Target = target;
                    await AttackSystem.Instance.InitiateAttack(attack);
                    return;
                }
            }

            ScenarioManager.inst.Surge(player);
        }
    }
}
