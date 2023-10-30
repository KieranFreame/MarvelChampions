using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Squirrel Girl", menuName = "MarvelChampions/Card Effects/Leadership/Squirrel Girl")]
public class SquirrelGirl : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        List<ICharacter> enemies = new();
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);
        enemies.Add(ScenarioManager.inst.ActiveVillain);

        await DamageSystem.Instance.ApplyDamage(new(enemies, 1, true, card: Card, owner: _owner));
    }
}
