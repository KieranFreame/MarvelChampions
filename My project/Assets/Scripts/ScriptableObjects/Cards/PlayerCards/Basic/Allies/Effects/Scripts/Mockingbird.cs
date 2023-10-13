using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mockingbird", menuName = "MarvelChampions/Card Effects/Basic/Allies/Mockingbird")]
public class Mockingbird : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        List<ICharacter> enemies = new()
        {
            ScenarioManager.inst.ActiveVillain,
        };
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        var target = (enemies.Count == 1) ? enemies[0] : await TargetSystem.instance.SelectTarget(enemies);

        target.CharStats.Attacker.Stunned = true;
    }
}
