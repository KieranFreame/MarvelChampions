using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Genetically Enhanced", menuName = "MarvelChampions/Card Effects/Nemesis/She-Hulk/Genetically Enhanced")]
public class GeneticallyEnhanced : AttachmentCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        int high = int.MinValue;

        foreach (MinionCard m in VillainTurnController.instance.MinionsInPlay)
        {
            if (m.CharStats.Health.BaseHP > high)
            {
                high = m.CharStats.Health.BaseHP;
                attached = m;
            }
        }

        if (attached != null)
        {
            Attach();
        }
        else
        {
            ScenarioManager.inst.Surge(player);
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }

        return Task.CompletedTask;
    }

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);

        Card.transform.SetParent((attached as MonoBehaviour).transform, false);
        Card.transform.SetAsFirstSibling();
        Card.transform.localPosition = new Vector3(-30, 0, 0);

        attached.CharStats.Health.IncreaseMaxHealth(3);
    }

    public override void Detach()
    {
        attached.Attachments.Remove(Card as IAttachment);
        attached.CharStats.Health.IncreaseMaxHealth(-3);
    }

    public override Task WhenDefeated()
    {
        if (attached != null)
            Detach();

        return Task.CompletedTask;
    }
}
