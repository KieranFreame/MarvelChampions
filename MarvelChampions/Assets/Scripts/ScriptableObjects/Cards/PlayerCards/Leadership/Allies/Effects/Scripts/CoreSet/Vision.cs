using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Vision", menuName = "MarvelChampions/Card Effects/Leadership/Vision")]
    public class Vision : PlayerCardEffect
    {
        bool increasedTHW;
        bool increasedATK;

        public override async Task OnEnterPlay()
        {
            increasedTHW = false;
            increasedATK = false;
            HasActivated = false;

            TurnManager.OnEndPlayerPhase += EndOfPhase;

            await Task.Yield();
        }

        public override bool CanActivate()
        {
            if (!_owner.Hand.cards.Any(x => x.Resources.Contains(Resource.Energy) || x.Resources.Contains(Resource.Wild)))
                return false;

            return !HasActivated;
        }

        public override async Task Activate()
        {
            await PayCostSystem.instance.GetResources(Resource.Energy, 1);

            int choice = await ChooseEffectUI.ChooseEffect(new List<string>() { "Increase THW by 2", "Increase ATK by 2" });

            if (choice == 1)
            {
                (Card as AllyCard).CharStats.Thwarter.CurrentThwart += 2;
                increasedTHW = true;
            }
            else
            {
                (Card as AllyCard).CharStats.Attacker.CurrentAttack += 2;
                increasedATK = true;
            }

            TurnManager.OnEndPlayerPhase += EndOfPhase;
            HasActivated = true;
        }

        private void EndOfPhase()
        {
            HasActivated = false;

            if (increasedATK)
            {
                (Card as AllyCard).CharStats.Attacker.CurrentAttack -= 2;
                increasedATK = false;
            }
            else if (increasedTHW)
            {
                (Card as AllyCard).CharStats.Thwarter.CurrentThwart -= 2;
                increasedTHW = false;
            }
        }

        public override void OnExitPlay()
        {
            TurnManager.OnEndPlayerPhase -= EndOfPhase;
        }
    }
}

