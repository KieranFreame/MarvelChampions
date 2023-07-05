using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Jennifer Walters", menuName = "MarvelChampions/Identity Effects/She-Hulk/Alter-Ego")]
    public class JenniferWalters : IdentityEffect, IModifyThreat
    {
        public override void LoadEffect(Player _owner)
        {
            owner = _owner;

            SchemeSystem.instance.Modifiers.Add(this);
            TurnManager.OnEndPlayerPhase += Reset;
        }

        public override void OnFlipDown()
        {
            SchemeSystem.instance.Modifiers.Remove(this);
        }

        public override void OnFlipUp()
        {
            SchemeSystem.instance.Modifiers.Add(this);
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

