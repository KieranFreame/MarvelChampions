using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Program Transmitter", menuName = "MarvelChampions/Card Effects/Ultron/Program Transmitter")]
public class ProgramTransmitter : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        GameStateManager.Instance.OnActivationCompleted += SchemeCompleted;

        return Task.CompletedTask;
    }

    private void SchemeCompleted(Action action)
    {
        if (action is not SchemeAction || action.Owner.Name != "Ultron") return;

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
        await PayCostSystem.instance.GetResources(new() { { Resource.Scientific, 2 } });
        player.Exhaust();

        GameStateManager.Instance.OnActivationCompleted -= SchemeCompleted;
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
}
