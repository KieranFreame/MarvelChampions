using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hysteria", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Hysteria")]
public class Hysteria : AttachmentCardEffect
{
    public override Task OnEnterPlay()
    {
        attached = _owner;

        Attach();

        return Task.CompletedTask;
    }

    private void ActivationInitiated()
    {
        BoostSystem.Instance.BoostCardCount++;
    }

    public override bool CanActivate(Player p)
    {
        return p.HaveResource(Resource.Scientific, 2);
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Scientific, 2 } });

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        attached.CharStats.AttackInitiated += ActivationInitiated;
        attached.CharStats.SchemeInitiated += ActivationInitiated;
    }

    public override void Detach()
    {
        attached.CharStats.AttackInitiated -= ActivationInitiated;
        attached.CharStats.SchemeInitiated -= ActivationInitiated;
    }
}
