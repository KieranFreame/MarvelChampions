using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Counter-Punch", menuName = "MarvelChampions/Card Effects/Protection/Counter-Punch")]
public class CounterPunch : PlayerCardEffect
{
    AttackAction attack;

    public override void OnDrawn(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;

        DefendSystem.instance.OnDefenderSelected += DefenderSelected;
    }

    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        AttackAction counter = new(player.CharStats.Attacker.CurrentAttack, attack.Owner, owner:_owner);
        await player.CharStats.InitiateAttack(counter);
    }

    private void DefenderSelected()
    {
        if (DefendSystem.instance.Target == _owner as ICharacter)
        {
            AttackSystem.OnAttackComplete += OnAttackComplete;
        }
    }

    private async void OnAttackComplete(Action action)
    {
        AttackSystem.OnAttackComplete -= OnAttackComplete;
        attack = action as AttackAction;

        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.instance.InitiatePlayCard(new(_owner, _owner.Hand.cards, Card));
        }
    }

    public override void OnDiscard()
    {
        DefendSystem.instance.OnDefenderSelected -= DefenderSelected;
    }
}
