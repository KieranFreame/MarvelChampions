using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield Block", menuName = "MarvelChampions/Card Effects/Captain America/Shield Block")]
public class ShieldBlock : PlayerCardEffect
{
    PlayerCard shield;

    public override void OnDrawn()
    {
        _owner.CharStats.Health.Modifiers.Add(OnTakeDamage);
    }

    public override Task OnEnterPlay()
    {
        shield.Exhaust();
        _owner.CharStats.Health.Modifiers.Remove(OnTakeDamage);
        return Task.CompletedTask;
    }

    public async Task<DamageAction> OnTakeDamage(DamageAction action)
    {
        if (!_owner.CardsInPlay.Permanents.Any(x => x.CardName == "Captain America's Shield"))
            return action;

        shield = _owner.CardsInPlay.Permanents.First(x => x.CardName == "Captain America's Shield");

        if (shield.Exhausted)
            return action;

        bool activate = await ConfirmActivateUI.MakeChoice(Card);

        if (activate)
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(Card));
            action.Value = 0;
        }

        return action;
    }

    public override void OnDiscard()
    {
        _owner.CharStats.Health.Modifiers.Remove(OnTakeDamage);
    }
}
