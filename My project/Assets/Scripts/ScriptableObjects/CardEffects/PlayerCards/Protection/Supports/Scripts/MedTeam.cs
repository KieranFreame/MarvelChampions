using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Med Team", menuName = "MarvelChampions/Card Effects/Protection/Med Team")]
public class MedTeam : PlayerCardEffect
{
    List<ICharacter> friendlies = new();
    private Counters counters;

    public override async Task OnEnterPlay(Player owner, PlayerCard card)
    {
        _owner = owner;
        Card = card;

        counters = Card.gameObject.AddComponent<Counters>();
        counters.AddCounters(3);

        await Task.Yield();
    }

    public override bool CanActivate()
    {
        friendlies.Add(_owner);
        friendlies.AddRange(_owner.CardsInPlay.Allies);

        friendlies.RemoveAll(x => !x.CharStats.Health.Damaged());

        return friendlies.Count > 0 && !Card.Exhausted;
    }

    public override async Task Activate()
    {
        Card.Exhaust();
        counters.RemoveCounters(1);

        var target = await TargetSystem.instance.SelectTarget(friendlies);

        target.CharStats.Health.RecoverHealth(2);

        if (counters.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(Card);
            _owner.Deck.Discard(Card);
        }
    }
}
