using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Vision", menuName = "MarvelChampions/Card Effects/Leadership/Vision")]
    public class Vision : PlayerCardEffect
    {
        public override async Task OnEnterPlay()
        {
            HasActivated = false;
            await Task.Yield();
        }

        public override bool CanActivate()
        {
            if (!_owner.HaveResource(Resource.Energy))
                return false;

            return !HasActivated;
        }

        public override async Task Activate()
        {
            await PayCostSystem.instance.GetResources(new() { { Resource.Energy, 1 } });

            int choice = await ChooseEffectUI.ChooseEffect(new List<string>() { "Increase THW by 2", "Increase ATK by 2" });

            if (choice == 1)
                (Card as AllyCard).CharStats.Thwarter.CurrentThwart += 2;
            else
                (Card as AllyCard).CharStats.Attacker.CurrentAttack += 2;
            

            TurnManager.OnEndPlayerPhase += EndOfPhase;
            HasActivated = true;
        }

        private void EndOfPhase()
        {
            HasActivated = false;
            (Card as AllyCard).CharStats.Attacker.CurrentAttack = (Card as AllyCard).CharStats.Attacker.BaseATK;
            (Card as AllyCard).CharStats.Thwarter.CurrentThwart = (Card as AllyCard).CharStats.Thwarter.BaseThwart;
        }

        public override void OnExitPlay()
        {
            TurnManager.OnEndPlayerPhase -= EndOfPhase;
        }
    }
}

