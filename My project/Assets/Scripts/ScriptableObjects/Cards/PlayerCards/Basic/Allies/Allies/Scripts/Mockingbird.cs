using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mockingbird", menuName = "MarvelChampions/Card Effects/Basic/Mockingbird")]
public class Mockingbird : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        List<ICharacter> enemies = new()
        {
            FindObjectOfType<Villain>()
        };
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        var target = await TargetSystem.instance.SelectTarget(enemies);

        target.CharStats.Attacker.Stunned = true;
    }
}
