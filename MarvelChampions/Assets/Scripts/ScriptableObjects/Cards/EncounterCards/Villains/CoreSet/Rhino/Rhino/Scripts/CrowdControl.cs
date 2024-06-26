using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Crowd Control", menuName = "MarvelChampions/Card Effects/Rhino/Crowd Control")]
public class CrowdControl : EncounterCardEffect
{
    Crisis _crisis;

    public override Task Resolve()
    {
        _crisis = new(_card as SchemeCard);
        return Task.CompletedTask;
    }
}
