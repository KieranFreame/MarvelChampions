using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Great Responsibility", menuName = "MarvelChampions/Card Effects/Justice/Great Responsibility")]
public class GreatResponsibility : PlayerCardEffect
{
    public override void OnDrawn()
    {
        SchemeSystem.Instance.Modifiers.Add(ModifyScheme);
    }

    public override bool CanBePlayed()
    {
        return false;
    }

    public async Task<SchemeAction> ModifyScheme(SchemeAction action)
    {
        if (_owner.Identity.ActiveIdentity is AlterEgo)
            return action;

        if (await ConfirmActivateUI.MakeChoice(Card))
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(_card));
            _owner.CharStats.Health.CurrentHealth -= action.Value;
            action.Value = 0;
            SchemeSystem.Instance.Modifiers.Remove(ModifyScheme);
        }

        return action;
    }

    public override void OnDiscard()
    {
        SchemeSystem.Instance.Modifiers.Remove(ModifyScheme);
    }
}
