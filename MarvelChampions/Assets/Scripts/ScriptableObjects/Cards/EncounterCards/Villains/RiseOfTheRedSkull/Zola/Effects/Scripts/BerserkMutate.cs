using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Berserk Mutate", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Berserk Mutate")]
public class BerserkMutate : EncounterCardEffect
{
    int counters;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is Hero)
        {
            await (card as MinionCard).CharStats.InitiateAttack();
        }
    }

    public override Task Boost(Action action)
    {
        Counters test = ScenarioManager.inst.MainScheme.GetComponent<Counters>();
        test.AddCounters(1);
        counters = test.CountersLeft;

        ScenarioManager.inst.ActiveVillain.CharStats.Attacker.CurrentAttack += counters;
        ScenarioManager.inst.ActiveVillain.CharStats.Schemer.CurrentScheme += counters;

        EffectResolutionManager.Instance.ResolvingEffects.Push(this);

        return Task.CompletedTask;
    }

    public override Task Resolve()
    {
        ScenarioManager.inst.ActiveVillain.CharStats.Attacker.CurrentAttack -= counters;
        ScenarioManager.inst.ActiveVillain.CharStats.Schemer.CurrentScheme -= counters;

        return Task.CompletedTask;
    }
}
