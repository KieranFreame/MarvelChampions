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

        private void SelectingDefender(Player target, AttackAction action)
        {
            if (action.Owner == ScenarioManager.inst.ActiveVillain as ICharacter && action.Card == null)
            {
                if (target == owner)
                    DrawCardSystem.Instance.DrawCards(new(1, owner));
            }
        }

        public override void OnFlipUp()
        {
            DefendSystem.Instance.OnSelectingDefender += SelectingDefender;
        }

        public override void OnFlipDown()
        {
            DefendSystem.Instance.OnSelectingDefender -= SelectingDefender;
        }
    }
}
