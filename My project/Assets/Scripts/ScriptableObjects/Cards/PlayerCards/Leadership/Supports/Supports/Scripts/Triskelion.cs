using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Triskelion", menuName = "MarvelChampions/Card Effects/Leadership/The Triskelion")]
public class Triskelion : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        _owner.CardsInPlay.AllyLimit++;

        await Task.Yield();
    }

    public override async void OnExitPlay()
    {
        _owner.CardsInPlay.AllyLimit--;

        if (_owner.CardsInPlay.ReachedAllyLimit())
        {
            await PlayCardSystem.instance.DiscardAlly();
        }
    }
}
