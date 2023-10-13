using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Jet-Trooper", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Hydra Assault/Hydra Jet-Trooper")]
public class HydraJetTrooper : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        await (card as MinionCard).CharStats.InitiateAttack();
    }

    public override Task Boost(Action action)
    {
        if (action is AttackAction)
        {
            AttackSystem.Instance.OnAttackCompleted.Add(ActivationComplete);
        }
        else if (TurnManager.instance.CurrPlayer.Identity.ActiveIdentity is Hero) //must be a scheme
        {
            SchemeSystem.Instance.SchemeComplete.Add(ActivationComplete);
        }

        return Task.CompletedTask;
    }

    private async Task ActivationComplete(Action action)
    {
        BoostSystem.Instance.BoostCardCount = 0;

        if (action is AttackAction)
            AttackSystem.Instance.OnAttackCompleted.Remove(ActivationComplete);
        else
            SchemeSystem.Instance.SchemeComplete.Remove(ActivationComplete);

        await ScenarioManager.inst.ActiveVillain.CharStats.InitiateAttack();
    }
}
