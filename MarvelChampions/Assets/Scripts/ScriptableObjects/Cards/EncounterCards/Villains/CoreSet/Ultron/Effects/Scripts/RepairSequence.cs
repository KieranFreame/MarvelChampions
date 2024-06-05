using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Repair Sequence", menuName = "MarvelChampions/Card Effects/Ultron/Repair Sequence")]
public class RepairSequence : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        int heal = VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Drone")).Count();
        heal *= 2;

        if (heal > 0)
        {
            owner.CharStats.Health.CurrentHealth += heal;
        }
        else
        {
            ScenarioManager.inst.Surge(player);
        }

        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        int heal = VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Drone")).Count();

        if (action.Owner is Villain)
            action.Owner.CharStats.Health.CurrentHealth += heal;
        else
            (action.Owner as MinionCard).Owner.CharStats.Health.CurrentHealth += heal;

        return Task.CompletedTask;
    }
}
