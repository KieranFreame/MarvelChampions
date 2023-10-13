using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sonic Converter", menuName = "MarvelChampions/Card Effects/Klaw/Sonic Converter")]
public class SonicConverter : EncounterCardEffect
{
    ICharacter _target;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        owner.CharStats.AttackInitiated += AttackInitiated;
        return Task.CompletedTask;
    }

    private void AttackInitiated() => DefendSystem.Instance.OnTargetSelected += SetDefender;

    private void SetDefender(ICharacter target)
    {
        DefendSystem.Instance.OnTargetSelected -= SetDefender;
        _target = target;
        _target.CharStats.Health.OnTakeDamage += OnTakeDamage;
    }

    private void OnTakeDamage(DamageAction action)
    {
        if (action.Value > 0)
        {
            _target.CharStats.Attacker.Stunned = true;
        }

        _target.CharStats.Health.OnTakeDamage -= OnTakeDamage;
    }

    public override bool CanActivate(Player p)
    {
        return (p.HaveResource(Resource.Energy) && p.HaveResource(Resource.Scientific) && p.HaveResource(Resource.Physical));
    }

    public override async Task Activate(Player player)
    {
        List<Task> tasks = new()
        {
            PayCostSystem.instance.GetResources(Resource.Energy, 1),
            PayCostSystem.instance.GetResources(Resource.Scientific, 1),
            PayCostSystem.instance.GetResources(Resource.Physical, 1)
        };

        await Task.WhenAll(tasks);

        _owner.CharStats.AttackInitiated -= AttackInitiated;
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
}
