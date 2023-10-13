using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Triskelion", menuName = "MarvelChampions/Card Effects/Leadership/The Triskelion")]
public class Triskelion : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CardsInPlay.AllyLimit++;
        return Task.CompletedTask;
    }

    public override async void OnExitPlay()
    {
        _owner.CardsInPlay.AllyLimit--;

        if (_owner.CardsInPlay.ReachedAllyLimit())
        {
            await PlayCardSystem.Instance.DiscardAlly();
        }
    }
}
