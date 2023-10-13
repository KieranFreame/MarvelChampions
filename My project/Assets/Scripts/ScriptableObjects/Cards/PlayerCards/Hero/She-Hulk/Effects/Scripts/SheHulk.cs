using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SheHulk", menuName = "MarvelChampions/Identity Effects/She-Hulk/Hero")]
public class SheHulk : IdentityEffect
{
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
    }

    public override async void OnFlipUp()
    {
        List<ICharacter> enemies = new() { FindObjectOfType<Villain>() };
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        if (enemies.Count == 0) return;

       await DamageSystem.Instance.ApplyDamage(new(enemies, 2));
    }
}
