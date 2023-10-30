using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hard as Nails", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/Hard as Nails")]
public class HardAsNails : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (!owner.CharStats.Health.Tough)
            owner.CharStats.Health.Tough = true;
        else if (owner.CharStats.Health.Damaged())
            owner.CharStats.Health.RecoverHealth(3);
        else
            ScenarioManager.inst.Surge(player);

        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        if (!ScenarioManager.inst.ActiveVillain.CharStats.Health.Tough)
            ScenarioManager.inst.ActiveVillain.CharStats.Health.Tough = true;
        else if (ScenarioManager.inst.ActiveVillain.CharStats.Health.Damaged())
            ScenarioManager.inst.ActiveVillain.CharStats.Health.RecoverHealth(3);

        return Task.CompletedTask;
    }
}
