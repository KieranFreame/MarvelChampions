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

    bool shouldSurge = false;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        if (shouldSurge)
        {
            villain.Surge(player);
            return;
        }

        identity = player.Identity;
        villain = owner;
        string databaseId = string.Concat(identity.Hero.Name.Where(c => !char.IsWhiteSpace(c))) + "Nemesis";

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
                continue;
            }

            ScenarioManager.inst.EncounterDeck.AddToDeck(Database.GetCardDataById(id)); 
        }

        shouldSurge = true;
    }
}
