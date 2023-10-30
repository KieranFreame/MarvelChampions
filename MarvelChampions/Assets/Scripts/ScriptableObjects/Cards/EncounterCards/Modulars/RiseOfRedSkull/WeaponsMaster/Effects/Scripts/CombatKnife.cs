using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Knife", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Weapon Master/Combat Knife")]
public class CombatKnife : AttachmentCardEffect
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
        return (player.HaveResource(Resource.Scientific) && player.HaveResource(Resource.Physical));
    }

    public override async Task Activate(Player p)
    {
        List<Task> tasks = new List<Task>()
        {
            PayCostSystem.instance.GetResources(Resource.Scientific, 1),
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
        _owner.CharStats.Attacker.Keywords.Add(Keywords.Piercing);
    }

    public override void Detach()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Attacker.Keywords.Remove(Keywords.Piercing);
    }
}
