using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Soldier", menuName = "MarvelChampions/Card Effects/Hydra/Hydra Soldier")]
public class HydraSoldier : EncounterCardEffect
{
    Guard _guard;
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;

        _guard = new(card as MinionCard);

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);

        return Task.CompletedTask;
    }
}
