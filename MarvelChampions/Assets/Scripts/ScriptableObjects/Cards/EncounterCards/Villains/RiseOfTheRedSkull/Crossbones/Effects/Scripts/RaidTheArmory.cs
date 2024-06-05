using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Raid the Armory", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/Raid the Armory")]
public class RaidTheArmory : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.MainScheme.Threat.GainThreat(1); //Incite 1;

        EncounterCardData data;

        do
        {
            data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
            ScenarioManager.inst.EncounterDeck.Mill(1);
        } while (!data.cardTraits.Contains("Weapon"));

        ScenarioManager.inst.EncounterDeck.limbo.Add(data);
        ScenarioManager.inst.EncounterDeck.discardPile.Remove(data);

        AttachmentCard weapon = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.AttachmentTransform) as AttachmentCard;
        ScenarioManager.inst.ActiveVillain.Attachments.Add((IAttachment)weapon.Effect);

        await weapon.OnRevealCard();
    }
}
