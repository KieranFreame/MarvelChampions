using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Zola", menuName = "MarvelChampions/Villain Effects/RotRS/Zola")]
public class Zola : VillainEffect
{
    Retaliate _retaliate;

    public override void LoadEffect(Villain owner)
    {
        base.LoadEffect(owner);

        _retaliate = new(owner, 1);
    }

    public override async Task StageTwoEffect()
    {
        var sideScheme = ScenarioManager.inst.EncounterDeck.Search("Test Subjects");

        if (sideScheme == null) return;

        SchemeCard TestSubjects = CreateCardFactory.Instance.CreateCard(sideScheme, GameObject.Find("SideSchemeTransform").transform) as SchemeCard;
        ScenarioManager.sideSchemes.Add(TestSubjects);
        await TestSubjects.Effect.OnEnterPlay(_owner, TestSubjects, null);
    }

    public override async Task StageThreeEffect()
    {
        List<EncounterCardData> data = new();
        data.AddRange(ScenarioManager.inst.EncounterDeck.deck.Where(x => x is MinionCardData).Cast<EncounterCardData>().ToList());
        data.AddRange(ScenarioManager.inst.EncounterDeck.discardPile.Where(x => x is MinionCardData).Cast<EncounterCardData>().ToList());

        MinionCard minion;

        List<MinionCard> minions = CardViewerUI.inst.EnablePanel(data.Cast<CardData>().ToList()).Cast<MinionCard>().ToList();

        foreach (var p in TurnManager.Players)
        {
            minion = await TargetSystem.instance.SelectTarget(minions);

            VillainTurnController.instance.MinionsInPlay.Add(minion);
            minion.transform.SetParent(GameObject.Find("MinionTransform").transform);

            await minion.Effect.WhenRevealed(_owner, minion, p);
        }
        
        CardViewerUI.inst.DisablePanel();
    }
}
