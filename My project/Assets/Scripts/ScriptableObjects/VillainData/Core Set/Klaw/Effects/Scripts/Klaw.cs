using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Klaw", menuName = "MarvelChampions/Villain Effects/Klaw")]
public class Klaw : VillainEffect
{
    public override void LoadEffect(Villain owner)
    {
        _owner = owner;
        _owner.CharStats.AttackInitiated += AttackInitiated;
    }

    private void AttackInitiated()
    {
        BoostSystem.Instance.BoostCardCount++;
    }

    public override async Task StageTwoEffect()
    {
        EncounterCardData immortalklaw = ScenarioManager.inst.EncounterDeck.Search("The Immortal Klaw", true) as EncounterCardData;

        if (immortalklaw == null)
            return;

        var card = CreateCardFactory.Instance.CreateCard(immortalklaw, GameObject.Find("SideSchemeTransform").transform) as SchemeCard;

        ScenarioManager.sideSchemes.Add(card);
        await card.OnRevealCard();
    }

    public override Task StageThreeEffect()
    {
        _owner.CharStats.Health.Tough = true;
        return Task.CompletedTask;
    }
}
