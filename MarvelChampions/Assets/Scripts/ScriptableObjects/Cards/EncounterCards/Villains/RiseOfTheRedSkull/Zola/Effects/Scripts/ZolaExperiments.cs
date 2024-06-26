using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Zola's Experiments", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Zola's Experiments")]
public class ZolaExperiments : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration++;

        RevealEncounterCardSystem.OnEncounterCardRevealed += AttachMinion;

        return Task.CompletedTask;
    }

    private void AttachMinion(EncounterCard card)
    {
        if (card is MinionCard)
        {
            var minion = card as MinionCard;
            CardData data = ScenarioManager.inst.EncounterDeck.discardPile.LastOrDefault(x => x.cardType == CardType.Attachment && x.cardTraits.Contains("Tech"));

            if (data != null)
            {
                ScenarioManager.inst.EncounterDeck.discardPile.Remove(data);
                ScenarioManager.inst.EncounterDeck.limbo.Add(data);

                AttachmentCard tech = CreateCardFactory.Instance.CreateCard(data, _card.transform) as AttachmentCard;

                tech.transform.SetAsFirstSibling();
                tech.transform.localPosition = new Vector3(-30, 0, 0);

                tech.Effect.Attach(_card as MinionCard, tech);

                minion.Attachments.Add(tech.Effect as IAttachment);
            }
        }
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration--;

        RevealEncounterCardSystem.OnEncounterCardRevealed -= AttachMinion;

        return Task.CompletedTask;
    }
}
