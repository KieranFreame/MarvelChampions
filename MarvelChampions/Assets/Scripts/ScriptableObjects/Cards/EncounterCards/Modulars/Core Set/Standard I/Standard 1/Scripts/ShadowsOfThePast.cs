using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Shadows of the Past", menuName = "MarvelChampions/Card Effects/Standard I/Shadows of the Past")]
public class ShadowsOfThePast : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;
        var identity = player.Identity;
        string databaseId = string.Concat(identity.Hero.Name.Where(c => !char.IsWhiteSpace(c))) + "Nemesis.txt";

        if (ScenarioManager.inst.SOTPResolved)
        {
            ScenarioManager.inst.Surge(player);
            return;
        }

        List<EncounterCardData> nemesisSet = TextReader.PopulateDeck("Nemesis/" + databaseId).Cast<EncounterCardData>().ToList();

        foreach (EncounterCardData data in nemesisSet)
        {
            if (data is MinionCardData && data.cardTraits.Contains("Nemesis"))
            {
                if (ScenarioManager.inst.EncounterDeck.Contains(data))
                {
                    ScenarioManager.inst.Surge(player);
                    continue;
                }

                MinionCard c = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
                VillainTurnController.instance.MinionsInPlay.Add(c);
                c.InPlay = true;
                await c.OnRevealCard();

                ScenarioManager.inst.EncounterDeck.limbo.Add(data);

                continue;
            }

            if (data is SchemeCardData)
            {
                if (ScenarioManager.inst.EncounterDeck.Contains(data))
                    continue;
                

                SchemeCard c = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("SideSchemeTransform").transform) as SchemeCard;
                ScenarioManager.sideSchemes.Add(c);
                c.InPlay = true;
                await c.OnRevealCard();

                ScenarioManager.inst.EncounterDeck.limbo.Add(data);

                continue;
            }

            ScenarioManager.inst.EncounterDeck.AddToDeck(data); 
        }

        ScenarioManager.inst.SOTPResolved = true;
    }
}
