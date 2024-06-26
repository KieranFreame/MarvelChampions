using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgraded Drones", menuName = "MarvelChampions/Card Effects/Ultron/Upgraded Drones")]
public class UpgradedDrones : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        foreach (var drone in VillainTurnController.instance.MinionsInPlay.Where(x => x.CardName == "Drone"))
        {
            drone.CharStats.Attacker.CurrentAttack++;
            drone.CharStats.Health.IncreaseMaxHealth(1);
        }

        VillainTurnController.instance.MinionsInPlay.CollectionChanged += MinionAdded;

        return Task.CompletedTask;
    }

    private void MinionAdded(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems)
            {
                if ((item as MinionCard).CardName == "Drone")
                {
                    (item as MinionCard).CharStats.Attacker.CurrentAttack++;
                    (item as MinionCard).CharStats.Health.IncreaseMaxHealth(1);
                }
            }
        }
    }

    public override bool CanActivate(Player player)
    {
        return (player.HaveResource(Resource.Energy) && player.HaveResource(Resource.Scientific) && player.HaveResource(Resource.Physical));
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Energy, 1 }, { Resource.Scientific, 1 }, { Resource.Physical, 1 } });

        foreach (var drone in VillainTurnController.instance.MinionsInPlay.Where(x => x.CardName == "Drone"))
        {
            drone.CharStats.Attacker.CurrentAttack--;
            drone.CharStats.Health.IncreaseMaxHealth(-1);
        }

        VillainTurnController.instance.MinionsInPlay.CollectionChanged -= MinionAdded;
        ScenarioManager.inst.EncounterDeck.Discard(Card);
        return;
    }
}
