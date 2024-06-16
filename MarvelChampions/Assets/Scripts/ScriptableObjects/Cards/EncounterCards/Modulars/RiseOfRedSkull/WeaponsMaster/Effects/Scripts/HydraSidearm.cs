using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Sidearm", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Weapon Master/Hydra Sidearm")]
public class HydraSidearm : AttachmentCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        Attach();
        return Task.CompletedTask;
    }

    public override bool CanActivate(Player player)
    {
        return (player.HaveResource(Resource.Energy) && player.HaveResource(Resource.Physical));
    }

    public override async Task Activate(Player player)
    {
        List<Task> tasks = new()
        {
            PayCostSystem.instance.GetResources(Resource.Energy, 1),
            PayCostSystem.instance.GetResources(Resource.Physical, 1)
        };

        foreach (Task t in tasks)
        {
            await t;
        }

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Attacker.Keywords.Add(Keywords.Ranged);
    }

    public override void Detach()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Attacker.Keywords.Remove(Keywords.Ranged);
    }
}
