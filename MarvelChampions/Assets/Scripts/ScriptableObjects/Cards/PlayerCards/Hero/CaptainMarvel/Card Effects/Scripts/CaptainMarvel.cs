using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Captain Marvel", menuName = "MarvelChampions/Identity Effects/Captain Marvel/Hero")]
    public class CaptainMarvel : IdentityEffect
    {
        public override void LoadEffect(Player _owner)
        {
            owner = _owner;
            hasActivated = false;

            TurnManager.OnStartPlayerPhase += Reset;
        }

        public override bool CanActivate()
        {
            if (!owner.CharStats.Health.Damaged())
            {
                return false;
            }

            if (owner.Hand.cards.FirstOrDefault(x => x.Resources.Contains(Resource.Energy)) == null) //need to account for Permanents
            {
                return false;
            }

            return !hasActivated;
        }

        public override void Activate()
        {
            Effect();
        }

        private async void Effect()
        {
            await PayCostSystem.instance.GetResources(Resource.Energy, 1);

            owner.CharStats.Health.CurrentHealth += 1;
            DrawCardSystem.Instance.DrawCards(new(1));

            hasActivated = true;
        }
    }
}
