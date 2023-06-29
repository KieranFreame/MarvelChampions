using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tenacity", menuName = "MarvelChampions/Card Effects/Basic/Tenacity")]
public class Tenacity : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;

        await Task.Yield();
    }

    public override bool CanActivate()
    {
        //TODO: Check inPlay supports and upgrades for resource generators
        return (_owner.Exhausted && _owner.Hand.cards.Any(x => x.Resources.Contains(Resource.Physical) || x.Resources.Contains(Resource.Wild)));
    }

    public override async Task Activate()
    {
        await PayCostSystem.instance.GetResources(Resource.Physical, 1);

        _owner.Ready();

        _owner.CardsInPlay.Permanents.Remove(Card);
        _owner.Deck.Discard(Card);
    }
}
