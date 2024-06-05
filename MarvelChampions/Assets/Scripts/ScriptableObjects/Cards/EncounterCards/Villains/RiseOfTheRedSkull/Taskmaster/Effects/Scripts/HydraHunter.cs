using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Hunter", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Hydra Hunter")]
public class HydraHunter : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        (card as MinionCard).CharStats.Attacker.Keywords.AddRange(new List<string>() { "Ranged", "Piercing" });
        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        if (TurnManager.instance.CurrPlayer.Identity.ActiveIdentity is Hero)
        {
            TurnManager.instance.CurrPlayer.CharStats.Health.TakeDamage(new(TurnManager.instance.CurrPlayer, 1));
        }
        else
        {
            ScenarioManager.inst.MainScheme.Threat.GainThreat(1);
        }

        return Task.CompletedTask;
    }
}
