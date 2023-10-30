using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Infinity Stone", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/The Infinity Stone")]
public class InfinityStone : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        WeaponsDeck.Instance.RevealTopWeapon();
        return Task.CompletedTask;
    }
}
