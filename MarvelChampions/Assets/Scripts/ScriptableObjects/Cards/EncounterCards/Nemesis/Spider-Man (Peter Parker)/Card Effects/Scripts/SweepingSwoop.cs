using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sweeping Swoop", menuName = "MarvelChampions/Card Effects/Nemesis/Spider-Man (Peter Parker)/Sweeping Swoop")]
public class SweepingSwoop : EncounterCardEffect
{
    public override Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;
        player.CharStats.Attacker.Stunned = true;

        if (VillainTurnController.instance.MinionsInPlay.FirstOrDefault(x => x.CardName == "The Vulture") != default)
            ScenarioManager.inst.Surge(player);

        return Task.CompletedTask;
    }

    public override async Task Boost(Action action)
    {
        if (action is not AttackAction)
            return;

        var attack = action as AttackAction;
        attack.Target.CharStats.Health.OnTakeDamage += OnTargetDamaged;

        await Task.Yield();
    }

    private void OnTargetDamaged(DamageAction arg0)
    {
        if (arg0.Value > 0)
            arg0.DamageTargets[0].CharStats.Attacker.Stunned = true;

        arg0.DamageTargets[0].CharStats.Health.OnTakeDamage -= OnTargetDamaged;
    }
}
