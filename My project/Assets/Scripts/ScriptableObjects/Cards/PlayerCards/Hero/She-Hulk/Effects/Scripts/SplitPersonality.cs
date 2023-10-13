using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Split Personality", menuName = "MarvelChampions/Card Effects/She-Hulk/Split Personality")]
public class SplitPersonality : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        bool HasFlipped = _owner.Identity.HasFlipped;
        _owner.Identity.Flip();
        _owner.Identity.HasFlipped = HasFlipped;

        while (_owner.Hand.cards.Count != _owner.Identity.ActiveIdentity.BaseHandSize)
            DrawCardSystem.Instance.DrawCards(new(1, _owner));

        await Task.Yield();
    }
}
