using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tac Team", menuName = "MarvelChampions/Card Effects/Aggression/Tac Team")]
public class TacTeam : PlayerCardEffect
{
    private Counters counters;

    public override async Task OnEnterPlay()
    {
        counters = Card.gameObject.AddComponent<Counters>();
        counters.AddCounters(3);

        await Task.Yield();
    }

    public override bool CanActivate()
    {
        return !Card.Exhausted;
    }

    public override async Task Activate()
    {
        Card.Exhaust();
        counters.RemoveCounters(1);

        List<ICharacter> enemies = new() { FindObjectOfType<Villain>() };
        enemies.AddRange(FindObjectsOfType<MinionCard>());
        await DamageSystem.instance.ApplyDamage(new(enemies, 2));

        if (counters.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(Card);
            _owner.Deck.Discard(Card);
        }
    }
}
