using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy Daggers", menuName = "MarvelChampions/Card Effects/Black Panther/Energy Daggers")]
public class EnergyDaggers : PlayerCardEffect, IBlackPanther
{
    public async Task Special(bool last)
    {
        List<ICharacter> enemies = new() { FindObjectOfType<Villain>() };
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        foreach (var character in enemies)
        {
            await DamageSystem.Instance.ApplyDamage(new(character, last ? 2 : 1, card:Card));
        }
    }
}
