using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Crossbones' Machine Gun", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/Crossbones' Machine Gun")]
public class CrossbonesMachineGun : EncounterCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    Counters _ammo;

    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        Card = card;
        Attached = _owner = owner;

        _ammo = _card.gameObject.AddComponent<Counters>();
        _ammo.AddCounters(2 * TurnManager.Players.Count);

        _owner.CharStats.Attacker.AttackCancel.Add(WhenAttacks);

        return Task.CompletedTask;
    }

    private async Task<AttackAction> WhenAttacks(AttackAction action)
    {
        _ammo.RemoveCounters(1);

        int boostIcons = (ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData).boostIcons;
        ScenarioManager.inst.EncounterDeck.Mill(1);

        List<ICharacter> targets = new() { TurnManager.instance.CurrPlayer };
        targets.AddRange((targets[0] as Player).CardsInPlay.Allies);

        await IndirectDamageHandler.inst.HandleIndirectDamage(targets, boostIcons);

        if (_ammo.CountersLeft <= 0)
        {
            _owner.CharStats.Attacker.AttackCancel.Remove(WhenAttacks);
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }

        return action;
    }

}
