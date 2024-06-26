using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TaskmasterScenario
{
    [CreateAssetMenu(fileName = "White Tiger", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Captives/White Tiger")]
    public class WhiteTiger : PlayerCardEffect
    {
        public override async Task OnEnterPlay()
        {
            if (Card.PrevZone == Zone.Hand)
            {
                /*CancellationToken token = CancelButton.ToggleCancelBtn(true, CancelEffect);
                await PayCostSystem.instance.GetResources(new() { { Resource.Scientific, 1 } }, true);
                CancelButton.ToggleCancelBtn(false, CancelEffect);*/

                await ThwartSystem.Instance.InitiateThwart(new(3, Card as ICharacter));
            }
        }

        private void CancelEffect()
        {
            CancelButton.ToggleCancelBtn(false, CancelEffect);
        }
    }
}

