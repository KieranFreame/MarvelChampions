using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TaskmasterScenario
{
    [CreateAssetMenu(fileName = "Shang-Chi", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Captives/Shang-Chi")]
    public class ShangChi : PlayerCardEffect
    {
        public override async Task OnEnterPlay()
        {
            if (Card.PrevZone == Zone.Hand)
            {
                CancellationToken token = CancelButton.ToggleCancelBtn(true, CancelEffect);
                await PayCostSystem.instance.GetResources(Resource.Energy, 1, true);
                CancelButton.ToggleCancelBtn(false, CancelEffect);

                List<ICharacter> enemies = new() { ScenarioManager.inst.ActiveVillain };
                enemies.AddRange(VillainTurnController.instance.MinionsInPlay);
                var target = await TargetSystem.instance.SelectTarget(enemies);
                target.CharStats.Attacker.Stunned = true;
            }
        }

        private void CancelEffect()
        {
            CancelButton.ToggleCancelBtn(false, CancelEffect);
        }
    }
}

