using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TaskmasterScenario
{
    [CreateAssetMenu(fileName = "Elektra", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Captives/Elektra")]
    public class Elektra : PlayerCardEffect
    {
        public override async Task OnEnterPlay()
        {
            if (Card.PrevZone == Zone.Hand)
            {
                /*CancellationToken token = CancelButton.ToggleCancelBtn(true, CancelEffect);
                await PayCostSystem.instance.GetResources(new() { { Resource.Physical, 1 } }, true);
                CancelButton.ToggleCancelBtn(false, CancelEffect);*/

                List<ICharacter> enemies = new() { ScenarioManager.inst.ActiveVillain };
                enemies.AddRange(VillainTurnController.instance.MinionsInPlay);
                await DamageSystem.Instance.ApplyDamage(new(enemies, 3, false, card: Card));
            }
        }

        private void CancelEffect()
        {
            CancelButton.ToggleCancelBtn(false, CancelEffect);
        }
    }
}

