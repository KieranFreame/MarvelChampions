using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Ivory Horn", menuName = "MarvelChampions/Card Effects/Rhino/Enhanced Ivory Horn")]
public class EnhancedIvoryHorn : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        _owner.CharStats.Attacker.CurrentAttack += 1;
        await Task.Yield();
    }

    public override bool CanActivate(Player player)
    {
        return player.HaveResource(Resource.Physical, 3);
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(resourceToCheck:Resource.Physical, amount:3);

        if (PayCostSystem.instance.Resources.Count >= 3)
        {
            _owner.CharStats.Attacker.CurrentAttack -= 1;
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }
    }
}
