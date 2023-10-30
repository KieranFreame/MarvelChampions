using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ground Stomp", menuName = "MarvelChampions/Card Effects/She-Hulk/Ground Stomp")]
public class GroundStomp : PlayerCardEffect
{
    readonly List<ICharacter> enemies = new();

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.Identity.ActiveIdentity is not Hero)
                return false;

            enemies.Clear();

            enemies.Add(FindObjectOfType<Villain>());
            enemies.AddRange(FindObjectsOfType<MinionCard>());

            return enemies.Count > 0;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        foreach (var enemy in enemies)
        {
           await DamageSystem.Instance.ApplyDamage(new(enemy, 1, card: Card));
        }
    }
}
