using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ground Stomp", menuName = "MarvelChampions/Card Effects/She-Hulk/Ground Stomp")]
public class GroundStomp : PlayerCardEffect
{
    List<ICharacter> enemies = new();

    public override bool CanBePlayed()
    {
        if (_owner.Identity.ActiveIdentity is not Hero)
            return false;

        enemies.Clear();

        enemies.Add(FindObjectOfType<Villain>());
        enemies.AddRange(FindObjectsOfType<MinionCard>());

        if (enemies.Count == 0) return false;

        return true;
    }

    public override async Task OnEnterPlay()
    {
        foreach (var enemy in enemies)
        {
           await DamageSystem.instance.ApplyDamage(new(enemy, 1));
        }
    }
}
