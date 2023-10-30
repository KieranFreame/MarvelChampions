using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Rhino", menuName = "MarvelChampions/Villain Effects/Rhino")]
public class Rhino : VillainEffect
{
    public override async Task StageTwoEffect()
    {
        EncounterCardData breakntake = ScenarioManager.inst.EncounterDeck.Search("Breakin' & Takin'", true) as EncounterCardData;

        if (breakntake == null)
            return;

        var card = CreateCardFactory.Instance.CreateCard(breakntake, GameObject.Find("SideSchemeTransform").transform) as SchemeCard;

        ScenarioManager.sideSchemes.Add(card);
        await card.OnRevealCard();
    }

    public override Task StageThreeEffect()
    {
        _owner.CharStats.Health.Tough = true;

        foreach (Player p in TurnManager.Players)
        {
            if (p.Identity.ActiveIdentity is Hero)
                p.CharStats.Attacker.Stunned = true;
        }

        return Task.CompletedTask;
    }
}
