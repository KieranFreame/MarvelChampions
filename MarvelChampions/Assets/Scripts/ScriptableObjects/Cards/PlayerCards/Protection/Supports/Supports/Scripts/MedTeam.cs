using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Med Team", menuName = "MarvelChampions/Card Effects/Protection/Supports/Med Team")]
public class MedTeam : PlayerCardEffect
{
    readonly List<ICharacter> friendlies = new();
    private Counters counters;

    public override async Task OnEnterPlay()
    {
        counters = _card.gameObject.AddComponent<Counters>();
        counters.AddCounters(3);

        await Task.Yield();
    }

    public override bool CanActivate()
    {
        friendlies.Add(_owner);
        friendlies.AddRange(_owner.CardsInPlay.Allies);

        friendlies.RemoveAll(x => !x.CharStats.Health.Damaged());

        return friendlies.Count > 0 && !_card.Exhausted;
    }

    public override async Task Activate()
    {
        _card.Exhaust();
        counters.RemoveCounters(1);

        var target = await TargetSystem.instance.SelectTarget(friendlies);

        target.CharStats.Health.CurrentHealth += 2;

        if (counters.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(_card);
            _owner.Deck.Discard(_card);
        }
    }
}
