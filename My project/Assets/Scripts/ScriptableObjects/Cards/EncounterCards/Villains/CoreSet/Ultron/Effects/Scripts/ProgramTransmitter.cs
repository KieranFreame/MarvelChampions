using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Program Transmitter", menuName = "MarvelChampions/Card Effects/Ultron/Program Transmitter")]
public class ProgramTransmitter : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        _owner.CharStats.SchemeInitiated += SchemeInitiated;

        return Task.CompletedTask;
    }

    private void SchemeInitiated()
    {
        SchemeSystem.Instance.SchemeComplete.Add(SchemeCompleted);
    }

    private Task SchemeCompleted(SchemeAction action)
    {
        SchemeSystem.Instance.SchemeComplete.Remove(SchemeCompleted);

        foreach (var scheme in ScenarioManager.sideSchemes)
        {
            scheme.Threat.GainThreat(1);
        }

        return Task.CompletedTask;
    }

    public override bool CanActivate(Player player)
    {
        if (player.Exhausted)
            return false;

        return true;
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(Resource.Scientific, 2);
        player.Exhaust();

        _owner.CharStats.SchemeInitiated -= SchemeInitiated;
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
}
