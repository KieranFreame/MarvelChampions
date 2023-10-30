using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Jennifer Walters", menuName = "MarvelChampions/Identity Effects/She-Hulk/Alter-Ego")]
    public class JenniferWalters : IdentityEffect
    {
        public override void LoadEffect(Player _owner)
        {
            owner = _owner;

            SchemeSystem.Instance.Modifiers.Add(ModifyScheme);
            TurnManager.OnEndPlayerPhase += Reset;
        }

        public override void OnFlipDown()
        {
            SchemeSystem.Instance.Modifiers.Remove(ModifyScheme);
        }

        public override void OnFlipUp()
        {
            SchemeSystem.Instance.Modifiers.Add(ModifyScheme);
        }

        public async Task<SchemeAction> ModifyScheme(SchemeAction action)
        {
            if (hasActivated)
                return action;

            bool decision = await ConfirmActivateUI.MakeChoice("Threat is being placed on a scheme.\nActivate Jennifer Walters' effect?");
            if (decision)
            {
                Debug.Log("Jennifer Walters: I Object!");
                action.Value--;
                hasActivated = true;
            }

            return action;
        }
    }
}

