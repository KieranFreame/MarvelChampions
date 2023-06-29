using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tac Team", menuName = "MarvelChampions/Card Effects/Aggression/Tac Team")]
public class TacTeam : PlayerCardEffect
{
    private Counters counters;

    /// <summary>
    /// Uses 3 counters. Exhaust Tac Team and remove 1 attack counter, deal 2 damage to an enemy
    /// </summary>

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
