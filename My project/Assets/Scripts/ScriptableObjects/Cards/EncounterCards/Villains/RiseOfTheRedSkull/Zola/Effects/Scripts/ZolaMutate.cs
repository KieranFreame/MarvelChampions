using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Zola's Mutate", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Zola's Mutate")]
public class ZolaMutate : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        CardData data;

        do
        {
            data = ScenarioManager.inst.EncounterDeck.deck[0];
            ScenarioManager.inst.EncounterDeck.Mill(1);
        } while (!data.cardTraits.Contains("Tech"));

        AttachmentCard tech = CreateCardFactory.Instance.CreateCard(data, card.transform) as AttachmentCard;

        tech.transform.SetAsFirstSibling();
        tech.transform.localPosition = new Vector3(-30, 0, 0);

        tech.Effect.Attach(card as MinionCard, tech);

        (card as MinionCard).Attachments.Add(tech);

        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        EncounterCardData zola = ScenarioManager.inst.EncounterDeck.discardPile.LastOrDefault(x => x.cardName == "Zola's Mutate") as EncounterCardData;

        if (zola != null)
        {
            ScenarioManager.inst.EncounterDeck.discardPile.Remove(zola);
            ScenarioManager.inst.EncounterDeck.AddToDeck(zola);
        }

        return Task.CompletedTask;
    }
}
