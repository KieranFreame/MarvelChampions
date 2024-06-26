using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sonic Converter", menuName = "MarvelChampions/Card Effects/Klaw/Sonic Converter")]
public class SonicConverter : EncounterCardEffect
{
    readonly Dictionary<Resource, int> cost = new()
    {
        {Resource.Energy, 1 },
        {Resource.Scientific, 1 },
        {Resource.Physical, 1 },
    };

    public override Task Resolve()
    {
        GameStateManager.Instance.OnActivationCompleted += CanRespond;
        return Task.CompletedTask;
    }

    private void CanRespond(Action arg0)
    {
        if (arg0 is not AttackAction || arg0.Owner.Name != "Klaw") return;

        ((AttackAction)arg0).Target.CharStats.Health.OnTakeDamage += Resolve;
    }

    private void Resolve(DamageAction action)
    {
        action.DamageTargets[0].CharStats.Health.OnTakeDamage -= Resolve;

        if (action.DamageTargets[0].CharStats.Health.CurrentHealth > 0 && action.Value > 0)
        {
            action.DamageTargets[0].CharStats.Attacker.Stunned = true;
        }
    }

    public override bool CanActivate(Player p)
    {
        return (p.HaveResource(Resource.Energy) && p.HaveResource(Resource.Scientific) && p.HaveResource(Resource.Physical));
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(cost);

        GameStateManager.Instance.OnActivationCompleted -= CanRespond;
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
}
