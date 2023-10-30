using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Getaway", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/The Getaway")]
public class Getaway : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        WeaponsDeck.Instance.RevealTopWeapon();
        return Task.CompletedTask;
    }
}
