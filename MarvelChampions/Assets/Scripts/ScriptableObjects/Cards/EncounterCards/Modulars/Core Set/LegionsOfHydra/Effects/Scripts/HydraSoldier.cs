using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Soldier", menuName = "MarvelChampions/Card Effects/Hydra/Hydra Soldier")]
public class HydraSoldier : EncounterCardEffect
{
    public override Task Resolve()
    {
        AttackSystem.Instance.Guards.Add((MinionCard)_card);
        GameStateManager.Instance.OnCharacterDefeated += WhenDefeated;
        return Task.CompletedTask;
    }

    public void WhenDefeated(ICharacter defeated)
    {
        if (defeated is not MinionCard || defeated as MinionCard != _card as MinionCard)
            return;

        AttackSystem.Instance.Guards.Remove((MinionCard)_card);
        GameStateManager.Instance.OnCharacterDefeated -= WhenDefeated;
        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
    }
}
