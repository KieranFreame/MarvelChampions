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
        base.OnDrawn(player, card);
        DefendSystem.instance.OnDefenderSelected += DefenderSelected;
    }

    public override async Task OnEnterPlay()
    {
        AttackAction counter = new(_owner.CharStats.Attacker.CurrentAttack, attack.Owner, owner:_owner);
        await _owner.CharStats.InitiateAttack(counter);
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
