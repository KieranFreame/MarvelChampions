using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Captain America's Helmet", menuName = "MarvelChampions/Card Effects/Captain America/Captain America's Helmet")]
public class CaptainAmericasHelmet : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        GameStateManager.Instance.OnCharacterDefeated += Defeated;

        return Task.CompletedTask;
    }

    private void Defeated(ICharacter player)
    {
        if (player !=(ICharacter) _owner) return;

        _owner.CharStats.Health.CurrentHealth = 1;
        _owner.Deck.Discard(Card);
    }

    public override void OnExitPlay()
    {
        GameStateManager.Instance.OnCharacterDefeated -= Defeated;
    }
}
