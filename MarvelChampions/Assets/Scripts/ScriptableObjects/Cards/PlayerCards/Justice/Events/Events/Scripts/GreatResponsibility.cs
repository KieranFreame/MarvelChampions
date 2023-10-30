using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Great Responsibility", menuName = "MarvelChampions/Card Effects/Justice/Great Responsibility")]
public class GreatResponsibility : PlayerCardEffect
{
    SchemeAction _action;

    public override void OnDrawn()
    {
        SchemeSystem.Instance.Modifiers.Add(ModifyScheme);
    }

    public override bool CanBePlayed()
    {
        return false;
    }

    public override Task OnEnterPlay()
    {
        _owner.CharStats.Health.CurrentHealth -= _action.Value;
        _action.Value = 0;
        SchemeSystem.Instance.Modifiers.Remove(ModifyScheme);

        return Task.CompletedTask;
    }

    public async Task<SchemeAction> ModifyScheme(SchemeAction action)
    {
        if (_owner.Identity.ActiveIdentity is AlterEgo)
            return action;

        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            _action = action;
            await PlayCardSystem.Instance.InitiatePlayCard(new(Card));
        }

        return action;
    }

    public override void OnDiscard()
    {
        SchemeSystem.Instance.Modifiers.Remove(ModifyScheme);
    }
}
