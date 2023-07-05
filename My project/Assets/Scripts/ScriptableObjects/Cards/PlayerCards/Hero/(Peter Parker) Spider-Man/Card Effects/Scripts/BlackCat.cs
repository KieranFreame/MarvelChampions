using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Black Cat", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Black Cat")]
public class BlackCat : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        for (int i = 0; i < 2; i++)
        {
            if ((_owner.Deck.deck[0] as PlayerCardData).cardResources.Contains(Resource.Scientific))
            {
                DrawCardSystem.instance.DrawCards(new(1, _owner));
            }
            else
            {
                Debug.Log("Black Cat discarded " + _owner.Deck.deck[0].cardName + " from your deck.");
                _owner.Deck.Mill(1);
            }
        }

        await Task.Yield();
    }
}
