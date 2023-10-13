using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Private Security Specialist", menuName = "MarvelChampions/Card Effects/Risky Business/Private Security Specialist")]
public class PrivateSecuritySpecialist : EncounterCardEffect
{
    Guard _guard;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _guard = new(card as MinionCard);
        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        if (ScenarioManager.inst.ActiveVillain.VillainName == "Norman Osborn")
        {
            RiskyBusiness.Instance.environment.AddCounters(1);
        }
        else
        {
            RiskyBusiness.Instance.environment.RemoveCounters(1);
        }

        return Task.CompletedTask;
    }
}
