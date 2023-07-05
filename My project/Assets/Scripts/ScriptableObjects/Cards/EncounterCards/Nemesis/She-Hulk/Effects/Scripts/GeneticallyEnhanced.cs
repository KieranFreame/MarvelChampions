using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Genetically Enhanced", menuName = "MarvelChampions/Card Effects/Nemesis/She-Hulk/Genetically Enhanced")]
public class GeneticallyEnhanced : EncounterCardEffect
{
    MinionCard attach;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        int high = int.MinValue;
        attach = null;

        foreach (MinionCard m in VillainTurnController.instance.MinionsInPlay)
        {
            if (m.CharStats.Health.BaseHP > high)
            {
                high = m.CharStats.Health.BaseHP;
                attach = m;
            }
        }

        if (attach != null)
        {
            attach.Attachments.Add(Card as IAttachment);

            Card.transform.SetParent(attach.transform, false);
            Card.transform.SetAsFirstSibling();
            Card.transform.localPosition = new Vector3(-30, 0, 0);

            attach.CharStats.Health.IncreaseMaxHealth(3);
        }
        else
        {
            owner.Surge(player);
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }

        await Task.Yield();
    }

    public override void OnExitPlay()
    {
        if (attach != null)
        {
            attach.Attachments.Remove(Card as IAttachment);
            attach.CharStats.Health.IncreaseMaxHealth(-3);
        }

    }
}
