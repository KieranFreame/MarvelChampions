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

    public override bool CanActivate()
    {
        return true;
    }

    public override async Task Activate()
    {
        await PayCostSystem.instance.GetResources(Resource.Physical, 3);

        if (PayCostSystem.instance.Resources.Count >= 3)
        {
            _owner.CharStats.Attacker.CurrentAttack -= 1;
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }
    }
}
