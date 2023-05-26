using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Shadows of the Past", menuName = "MarvelChampions/Card Effects/Standard I/Shadows of the Past")]
public class ShadowsOfThePast : CardEffect
{
    Identity identity;
    Villain villain;

    bool shouldSurge = false;

    public override void OnEnterPlay(Villain owner, Card card)
    {
        identity = FindObjectOfType<Player>().Identity;
        villain = owner;
        string databaseId = string.Concat(identity.Hero.Name.Where(c => !char.IsWhiteSpace(c))) + "Nemesis";

        if (Database.GetCardDataById(Database.GetCardSetByName(databaseId).cardIDs[0]) == null)
            return;

        foreach (string id in Database.GetCardSetByName(databaseId).cardIDs)
        {
            if (villain.EncounterDeck.Contains(Database.GetCardDataById(id)))
            {
                if (id.Contains("-M-"))
                    shouldSurge = true;

                continue;
            }

            if (id.Contains("-M-") || id.Contains("-SS-")) //minion or sidescheme
            {
                GameObject c = RevealCardSystem.instance.CreateEncounterCard(Database.GetCardDataById(id) as EncounterCardData, false);
                RevealEncounterCardSystem.instance.InitiateRevealCard(c.GetComponent<EncounterCard>());
            }
            else
            {
                villain.EncounterDeck.AddToDeck(Database.GetCardDataById(id));
            }
                
        }

        if (shouldSurge)
            villain.Surge(FindObjectOfType<Player>());
    }
}
