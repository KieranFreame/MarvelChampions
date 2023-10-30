using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "Sweeping Swoop", menuName = "MarvelChampions/Card Effects/Nemesis/Spider-Man (Peter Parker)/Sweeping Swoop")]
public class SweepingSwoop : EncounterCardEffect
{
    int charHp;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        player.CharStats.Attacker.Stunned = true;

        if (VillainTurnController.instance.MinionsInPlay.FirstOrDefault(x => x.CardName == "The Vulture") != default)
        {
            ScenarioManager.inst.Surge(player);
        }

        await Task.Yield();
    }

    public override async Task Boost(Action action)
    {
        if (action is not AttackAction)
            return;

        var attack = action as AttackAction;

        charHp = attack.Target.CharStats.Health.CurrentHealth;
        AttackSystem.Instance.OnAttackCompleted.Add(AttackComplete);

        await Task.Yield();
    }

    private Task AttackComplete(Action action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackComplete);
        var attack = action as AttackAction;

        if (attack.Target.CharStats.Health.CurrentHealth < charHp)
            attack.Target.CharStats.Attacker.Stunned = true;

        return Task.CompletedTask;
    }
}
