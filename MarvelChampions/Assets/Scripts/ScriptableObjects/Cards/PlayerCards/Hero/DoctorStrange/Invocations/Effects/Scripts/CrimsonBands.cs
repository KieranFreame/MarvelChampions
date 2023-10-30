using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Crimson Bands", menuName = "MarvelChampions/Card Effects/Doctor Strange/Invocations/Crimson Bands")]
public class CrimsonBands : PlayerCardEffect, IInvocation
{
    public override bool CanActivate()
    {
        return true;
    }

    public async Task Special()
    {
        List<ICharacter> enemies = new()
        {
            ScenarioManager.inst.ActiveVillain,
        };
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        var target = (enemies.Count == 1) ? enemies[0] : await TargetSystem.instance.SelectTarget(enemies);

        target.CharStats.Attacker.Stunned = true;
        target.CharStats.Health.TakeDamage(new(target, 7));
    }
}
