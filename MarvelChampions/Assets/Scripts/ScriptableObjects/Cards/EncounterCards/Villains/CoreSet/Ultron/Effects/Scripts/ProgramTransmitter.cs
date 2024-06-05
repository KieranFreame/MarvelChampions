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

        SchemeSystem.Instance.SchemeComplete.Add(SchemeCompleted);

        return Task.CompletedTask;
    }

    private void SchemeCompleted(SchemeAction action)
    {
        foreach (var scheme in ScenarioManager.sideSchemes)
        {
            scheme.Threat.GainThreat(1);
        }
    }

    public override bool CanActivate(Player player)
    {
        return (!player.Exhausted && player.HaveResource(Resource.Scientific, 2));
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(Resource.Scientific, 2);
        player.Exhaust();

        SchemeSystem.Instance.SchemeComplete.Remove(SchemeCompleted);
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
}
