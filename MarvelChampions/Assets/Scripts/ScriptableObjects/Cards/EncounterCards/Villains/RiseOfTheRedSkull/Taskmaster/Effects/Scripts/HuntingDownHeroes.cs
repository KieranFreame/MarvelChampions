using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hunting Down Heroes", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Hunting Down Heroes")]
public class HuntingDownHeroes : EncounterCardEffect
{
    public static List<AllyCardData> captives;

    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        captives = new List<AllyCardData>()
        {
            Database.GetCardDataById("CAPTIVE-A-001") as AllyCardData,
            Database.GetCardDataById("CAPTIVE-A-002") as AllyCardData,
            Database.GetCardDataById("CAPTIVE-A-003") as AllyCardData,
            Database.GetCardDataById("CAPTIVE-A-004") as AllyCardData,
        };

        var sideScheme = ScenarioManager.inst.EncounterDeck.Search("Hydra Patrol");

        SchemeCard HydraPatrol = CreateCardFactory.Instance.CreateCard(sideScheme, GameObject.Find("SideSchemeTransform").transform) as SchemeCard;
        ScenarioManager.sideSchemes.Add(HydraPatrol);
        await HydraPatrol.Effect.OnEnterPlay(owner, HydraPatrol, null);

        (Card as MainSchemeCard).AfterStepOne.Add(AfterStepOne);
    }

    public static AllyCardData GetCaptive()
    {
        if (captives.Count == 0)
        {
            Debug.Log("All Heroes Freed");
            return null;
        }

        var captive = captives[Random.Range(0, captives.Count)];
        captives.Remove(captive);

        return captive;
    }

    private async Task AfterStepOne()
    {
        foreach (var p in TurnManager.Players.Where(x => x.Identity.ActiveIdentity is Hero))
        {
            int choice = await ChooseEffectUI.ChooseEffect(new() { "Hunting Down Heroes gains +1 threat", "Take 1 damage" });

            switch (choice)
            {
                case 1:
                    (Card as SchemeCard).Threat.GainThreat(1);
                    break;
                case 2:
                    p.CharStats.Health.TakeDamage(new(p, 1));
                    break;
            }
        }
    }
}
