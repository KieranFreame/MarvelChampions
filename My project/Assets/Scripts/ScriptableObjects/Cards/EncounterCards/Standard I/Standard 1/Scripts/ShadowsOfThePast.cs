using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Shadows of the Past", menuName = "MarvelChampions/Card Effects/Standard I/Shadows of the Past")]
public class ShadowsOfThePast : EncounterCardEffect
{
    Identity identity;
    Villain villain;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        identity = player.Identity;
        villain = owner;
        string databaseId = string.Concat(identity.Hero.Name.Where(c => !char.IsWhiteSpace(c))) + "Nemesis";

        if (ScenarioManager.inst.SOTPResolved.Contains(player))
        {
            villain.Surge(player);
            return;
        }

        if (Database.GetCardSetByName(databaseId) == null)
        {
            Debug.Log("Nemesis Set Not Implemented");
            villain.Surge(player);
            return;
        }
            

        if (Database.GetCardDataById(Database.GetCardSetByName(databaseId).cardIDs[0]) == null)
            return;

        foreach (string id in Database.GetCardSetByName(databaseId).cardIDs)
        {
            CardData data = Database.GetCardDataById(id);

            if (data is MinionCardData)
            {
                if (ScenarioManager.inst.EncounterDeck.Contains(data) && data.cardTraits.Contains("Nemesis"))
                {
                    villain.Surge(player);
                    continue;
                }

                GameObject c = RevealCardSystem.instance.CreateEncounterCard(Database.GetCardDataById(id) as EncounterCardData, false);
                await RevealEncounterCardSystem.instance.InitiateRevealCard(c.GetComponent<EncounterCard>());

                ScenarioManager.inst.EncounterDeck.limbo.Add(data);

                continue;
            }

            if (data is SchemeCardData)
            {
                if (ScenarioManager.inst.EncounterDeck.Contains(data))
                {
                    continue;
                }

                GameObject c = RevealCardSystem.instance.CreateEncounterCard(Database.GetCardDataById(id) as EncounterCardData, false);
                await RevealEncounterCardSystem.instance.InitiateRevealCard(c.GetComponent<EncounterCard>());

                ScenarioManager.inst.EncounterDeck.limbo.Add(data);

                continue;
            }

            ScenarioManager.inst.EncounterDeck.AddToDeck(Database.GetCardDataById(id)); 
        }

        ScenarioManager.inst.SOTPResolved.Add(player);
    }
}
