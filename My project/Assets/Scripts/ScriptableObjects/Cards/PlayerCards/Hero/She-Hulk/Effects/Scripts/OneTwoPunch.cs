using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "One-Two Punch", menuName = "MarvelChampions/Card Effects/She-Hulk/One-Two Punch")]
public class OneTwoPunch : PlayerCardEffect
{
    public override void OnDrawn()
    {
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

        AttackSystem.Instance.OnAttackCompleted.Add(OnAttackComplete);
    }

    private async Task OnAttackComplete(Action action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(OnAttackComplete);

        if (_owner.ResourcesAvailable(Card) < Card.CardCost || !_owner.Exhausted) return;

        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(Card));
        }
    }

    public override void OnDiscard()
    {
        FindObjectOfType<IdentityActions>(true).OnBasicAttack -= OnBasicAttack;
    }
}
