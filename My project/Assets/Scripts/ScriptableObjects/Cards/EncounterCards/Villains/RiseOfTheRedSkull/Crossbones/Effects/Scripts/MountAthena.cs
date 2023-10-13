using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack on Mount Athena", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/Attack on Mount Athena")]
public class MountAthena : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        WeaponsDeck.Instance.RevealTopWeapon();
        return Task.CompletedTask;
    }
}
