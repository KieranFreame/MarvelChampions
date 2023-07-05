using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "One-Two Punch", menuName = "MarvelChampions/Card Effects/She-Hulk/One-Two Punch")]
public class OneTwoPunch : PlayerCardEffect
{
    public override void OnDrawn(Player player, PlayerCard card)
    {
        base.OnDrawn(player, card);

        FindObjectOfType<IdentityActions>(true).OnBasicAttack += OnBasicAttack;
    }

    public override async Task OnEnterPlay()
    {
        FindObjectOfType<IdentityActions>(true).OnBasicAttack -= OnBasicAttack;
        _owner.Ready();

        await Task.Yield();
    }

    private void OnBasicAttack()
    {
        if (_owner.CharStats.Attacker.Stunned)
            return;

        AttackSystem.OnActivationComplete += OnActivationComplete;
    }

    private async void OnActivationComplete()
    {
        AttackSystem.OnActivationComplete -= OnActivationComplete;

        if (_owner.ResourcesAvailable(Card) < Card.CardCost) return;

        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.instance.InitiatePlayCard(new(_owner, _owner.Hand.cards, Card));
        }
    }

    public override void OnDiscard()
    {
        FindObjectOfType<IdentityActions>(true).OnBasicAttack -= OnBasicAttack;
    }
}
