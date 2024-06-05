using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tac Team", menuName = "MarvelChampions/Card Effects/Aggression/Tac Team")]
public class TacTeam : PlayerCardEffect
{
    private Counters counters;

    public override Task OnEnterPlay()
    {
        counters = _card.gameObject.AddComponent<Counters>();
        counters.AddCounters(3);

        return Task.CompletedTask;
    }

    public override bool CanActivate()
    {
        return !_card.Exhausted && ScenarioManager.inst.ActiveVillain != null; //Sinister Six scenario
    }

    public override async Task Activate()
    {
        _card.Exhaust();
        counters.RemoveCounters(1);

        List<ICharacter> enemies = new() { ScenarioManager.inst.ActiveVillain };
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);
        await DamageSystem.Instance.ApplyDamage(new(enemies, 2, card: Card));

        if (counters.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(_card);
            _owner.Deck.Discard(Card);
        }
    }
}
