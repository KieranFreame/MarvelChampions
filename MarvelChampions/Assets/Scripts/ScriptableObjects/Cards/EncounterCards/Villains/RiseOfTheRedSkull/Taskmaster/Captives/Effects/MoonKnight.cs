using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TaskmasterScenario
{
    [CreateAssetMenu(fileName = "Moon Knight", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Captives/Moon Knight")]
    public class MoonKnight : PlayerCardEffect
    {
        public override Task OnEnterPlay()
        {
            if (Card.PrevZone == Zone.Hand && _owner.HaveResource(Resource.Wild))
                EffectManager.Inst.Responding.Add(this);

            return Task.CompletedTask;
        }

        public override async Task Resolve()
        {
            await PayCostSystem.instance.GetResources(new() { { Resource.Wild, 1 } });
            DrawCardSystem.Instance.DrawCards(new(2, _owner));
        }
    }
}


