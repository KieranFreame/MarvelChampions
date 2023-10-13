using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Crossbones' Armor", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/Crossbones' Armor")]
public class CrossbonesArmor : AttachmentCardEffect
{
    private Counters counters;
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        counters = Card.gameObject.AddComponent<Counters>();
        _owner.CharStats.Health.Modifiers.Add(OnTakeDamage);

        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Schemer.CurrentScheme++;

        await Task.Yield();
    }

    public Task<DamageAction> OnTakeDamage(DamageAction action)
    {
        counters.AddCounters(action.Value);

        if (counters.CountersLeft >= 5)
        {
            _owner.CharStats.Health.Modifiers.Remove(OnTakeDamage);

            _owner.CharStats.Attacker.CurrentAttack--;
            _owner.CharStats.Schemer.CurrentScheme--;

            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }

        action.Value = 0;
        return Task.FromResult(action);
    }
}
