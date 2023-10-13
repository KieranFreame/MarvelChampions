using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Indomitable", menuName = "MarvelChampions/Card Effects/Protection/Upgrades/Indomitable")]
public class Indomitable : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        DefendSystem.Instance.OnDefenderSelected += DefenderSelected;
        await Task.Yield();
    }

    private void DefenderSelected(ICharacter target)
    {
        if (target == null) return;

        if (target == _owner as ICharacter)
            AttackSystem.Instance.OnAttackCompleted.Add(OnAttackComplete);
    }

    private async Task OnAttackComplete(Action action)
    {
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            _owner.Ready();

            _owner.CardsInPlay.Permanents.Remove(Card);
            _owner.Deck.Discard(Card);
            DefendSystem.Instance.OnDefenderSelected -= DefenderSelected;
        }
    }

    public override void OnExitPlay()
    {
        DefendSystem.Instance.OnDefenderSelected -= DefenderSelected;
    }
}
