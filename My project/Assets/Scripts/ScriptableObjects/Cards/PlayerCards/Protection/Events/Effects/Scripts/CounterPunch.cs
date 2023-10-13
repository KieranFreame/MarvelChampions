using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Counter-Punch", menuName = "MarvelChampions/Card Effects/Protection/Events/Counter-Punch")]
public class CounterPunch : PlayerCardEffect
{
    AttackAction attack;

    public override void OnDrawn()
    {
        DefendSystem.Instance.OnDefenderSelected += DefenderSelected;
    }

    public override async Task OnEnterPlay()
    {
        AttackAction counter = new(_owner.CharStats.Attacker.CurrentAttack, attack.Owner, owner:_owner, card:Card);
        await _owner.CharStats.InitiateAttack(counter);
    }

    private void DefenderSelected(ICharacter target)
    {
        if (target == null) return;

        if (target == _owner as ICharacter)
            AttackSystem.Instance.OnAttackCompleted.Add(OnAttackComplete);
    }

    private async Task OnAttackComplete(Action action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(OnAttackComplete);
        attack = action as AttackAction;

        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(Card));
        }
    }

    public override void OnDiscard()
    {
        DefendSystem.Instance.OnDefenderSelected -= DefenderSelected;
    }
}
