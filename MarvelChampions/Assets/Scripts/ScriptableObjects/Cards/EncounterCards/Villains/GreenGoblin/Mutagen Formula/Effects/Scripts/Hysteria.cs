using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hysteria", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Hysteria")]
public class Hysteria : AttachmentCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        attached = _owner = owner;
        Card = card;

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
        await PayCostSystem.instance.GetResources(Resource.Scientific, 2);

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
