using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Spider-Man (Peter Parker)", menuName = "MarvelChampions/Identity Effects/Spider-Man (Peter Parker)/Hero")]
    public class SpiderMan : IdentityEffect
    {
        public override void LoadEffect(Player _owner)
        {
            owner = _owner;
        }

        private void AttackInitiated()
        {
            DefendSystem.Instance.OnSelectingDefender += SelectingDefender;
        }

        private void SelectingDefender(Player target)
        {
            DefendSystem.Instance.OnSelectingDefender -= SelectingDefender;

            if (target == owner)
                DrawCardSystem.Instance.DrawCards(new(1, owner));
        }

        public override void OnFlipUp()
        {
            FindObjectOfType<Villain>().CharStats.AttackInitiated += AttackInitiated;
        }

        public override void OnFlipDown()
        {
            FindObjectOfType<Villain>().CharStats.AttackInitiated -= AttackInitiated;
        }
    }
}
