using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Backflip", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Backflip")]
public class Backflip : PlayerCardEffect, IModifyDamage
{
    public override void OnDrawn(Player player, PlayerCard card)
    {
        base.OnDrawn(player, card);
        _owner.CharStats.Health.Modifiers.Add(this);
    }

    public override async Task OnEnterPlay()
    {
        _owner.CharStats.Health.Modifiers.Remove(this);
        await Task.Yield();
    }

    public async Task<int> OnTakeDamage(int damage)
    {
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.instance.InitiatePlayCard(new(_owner, _owner.Hand.cards, Card));
            return 0;
        }

        return damage;
    }

    public override void OnDiscard()
    {
        _owner.CharStats.Health.Modifiers.Remove(this);
    }
}
