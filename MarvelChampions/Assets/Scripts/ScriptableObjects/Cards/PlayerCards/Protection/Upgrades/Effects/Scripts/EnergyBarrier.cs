using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy Barrier", menuName = "MarvelChampions/Card Effects/Protection/Upgrades/Energy Barrier")]
public class EnergyBarrier : PlayerCardEffect
{
    Counters energy;

    public override Task OnEnterPlay()
    {
        energy = _card.gameObject.AddComponent<Counters>();
        energy.AddCounters(3);

        _owner.CharStats.Health.Modifiers.Add(ModifyDamage);

        return Task.CompletedTask;
    }

    private async Task<DamageAction> ModifyDamage(DamageAction action)
    {
        bool choice = await ConfirmActivateUI.MakeChoice(_card);

        if (choice)
        {
            action.Value--;
            energy.RemoveCounters(1);

            List<ICharacter> enemies = new() { ScenarioManager.inst.ActiveVillain };
            enemies.AddRange(VillainTurnController.instance.MinionsInPlay);
            await DamageSystem.Instance.ApplyDamage(new(enemies, 1, card: _card));

            if (energy.CountersLeft == 0)
            {
                _owner.CharStats.Health.Modifiers.Remove(ModifyDamage);
                _owner.CardsInPlay.Permanents.Remove(_card);
                _owner.Deck.Discard(_card);
            }
        }

        return action;
    }
}
