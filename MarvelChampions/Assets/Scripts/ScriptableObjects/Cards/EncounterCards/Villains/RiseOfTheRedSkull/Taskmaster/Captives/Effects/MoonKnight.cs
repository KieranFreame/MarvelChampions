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
        public override async Task OnEnterPlay()
        {
            if (Card.PrevZone == Zone.Hand)
            {
                CancellationToken token = CancelButton.ToggleCancelBtn(true, CancelEffect);
                await PayCostSystem.instance.GetResources(Resource.Wild, 1, true);
                CancelButton.ToggleCancelBtn(false, CancelEffect);

                DrawCardSystem.Instance.DrawCards(new(2, _owner));
            }
        }

        private void CancelEffect()
        {
            CancelButton.ToggleCancelBtn(false, CancelEffect);
        }
    }
}


