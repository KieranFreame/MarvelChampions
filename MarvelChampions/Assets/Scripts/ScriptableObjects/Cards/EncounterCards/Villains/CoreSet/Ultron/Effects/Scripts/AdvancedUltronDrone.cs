using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Advanced Ultron Drone", menuName = "MarvelChampions/Card Effects/Ultron/Advanced Ultron Drone")]
public class AdvancedUltronDrone : EncounterCardEffect
{
    UltronDrones ultronDrones;

    public override Task OnEnterPlay()
    {
        AttackSystem.Instance.Guards.Add((MinionCard)_card);
        GameStateManager.Instance.OnCharacterDefeated += WhenDefeated;
        ultronDrones = GameObject.Find("Ultron Drones").GetComponent<EncounterCard>().Effect as UltronDrones;

        return Task.CompletedTask;
    }

    public void WhenDefeated(ICharacter defeated)
    {
        if (defeated is not MinionCard || defeated as MinionCard != _card as MinionCard)
            return;

        AttackSystem.Instance.Guards.Remove((MinionCard)_card);
        GameStateManager.Instance.OnCharacterDefeated -= WhenDefeated;

        ultronDrones.SpawnDrone(TurnManager.instance.CurrPlayer);
    }
}
