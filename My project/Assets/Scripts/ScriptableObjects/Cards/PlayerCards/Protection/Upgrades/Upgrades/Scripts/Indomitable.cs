using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Indomitable", menuName = "MarvelChampions/Card Effects/Protection/Indomitable")]
public class Indomitable : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        DefendSystem.instance.OnDefenderSelected += DefenderSelected;
        await Task.Yield();
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
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            _owner.Ready();

            _owner.CardsInPlay.Permanents.Remove(Card);
            _owner.Deck.Discard(Card);
            DefendSystem.instance.OnDefenderSelected -= DefenderSelected;
        }
    }

    public override void OnExitPlay()
    {
        DefendSystem.instance.OnDefenderSelected -= DefenderSelected;
    }
}
