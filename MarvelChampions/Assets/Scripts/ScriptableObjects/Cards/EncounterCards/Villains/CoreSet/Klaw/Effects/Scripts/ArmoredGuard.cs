using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Armored Guard", menuName = "MarvelChampions/Card Effects/Klaw/Armored Guard")]
public class ArmoredGuard : EncounterCardEffect
{
    public override Task Resolve()
    {
        AttackSystem.Instance.Guards.Add((MinionCard)_card);
        GameStateManager.Instance.OnCharacterDefeated += WhenDefeated;
        (_card as MinionCard).CharStats.Health.Tough = true;

        return Task.CompletedTask;
    }

    public void WhenDefeated(ICharacter defeated)
    {
        if (defeated is not MinionCard || defeated as MinionCard != _card as MinionCard)
            return;

        AttackSystem.Instance.Guards.Remove((MinionCard)_card);
        GameStateManager.Instance.OnCharacterDefeated -= WhenDefeated;
    }
}
