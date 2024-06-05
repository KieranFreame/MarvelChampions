using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Counter-Punch", menuName = "MarvelChampions/Card Effects/Protection/Events/Counter-Punch")]
public class CounterPunch : PlayerCardEffect, IOptional
{
    AttackAction attack;

    public override void OnDrawn()
    {
        DefendSystem.Instance.OnDefenderSelected += DefenderSelected;
    }

    private void DefenderSelected(ICharacter target, AttackAction action)
    {
        if (target != _owner as ICharacter) return;
            EffectResolutionManager.Instance.ResolvingEffects.Push(this);
    }

    public override async Task OnEnterPlay()
    {
        AttackAction counter = new(_owner.CharStats.Attacker.CurrentAttack, attack.Owner, owner: _owner, card: _card);
        await _owner.CharStats.InitiateAttack(counter);
    }

    public override async Task Resolve()
    {
        attack = AttackSystem.Instance.Action;
        await PlayCardSystem.Instance.InitiatePlayCard(new(_card));
        DefendSystem.Instance.OnDefenderSelected -= DefenderSelected;
    }

    public override void OnDiscard()
    {
        DefendSystem.Instance.OnDefenderSelected -= DefenderSelected;
    }
}
