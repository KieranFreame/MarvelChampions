using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Clea", menuName = "MarvelChampions/Card Effects/Protection/Allies/Clea")]
public class Clea : PlayerCardEffect
{
    public override async Task WhenDefeated()
    {
        bool activate = await ConfirmActivateUI.MakeChoice(Card);

        if (activate)
        {
            CardData d = _owner.Deck.limbo.FirstOrDefault(x => x.cardName == "Clea");
            _owner.Deck.limbo.Remove(d);
            _owner.Deck.AddToDeck(d);
            Destroy(_card.gameObject);
        }
    }
}
