using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Nemesis
{
    [CreateAssetMenu(fileName = "The Vulture", menuName = "MarvelChampions/Card Effects/Nemesis/Spider-Man (Peter Parker)/Vulture")]
    public class Vulture : EncounterCardEffect
    {
        public override async Task OnEnterPlay()
        {
            await ((MinionCard)_card).CharStats.InitiateAttack();
        }
    }
}

