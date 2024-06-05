using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Biomechanical Upgrades", menuName = "MarvelChampions/Card Effects/The Doomsday Chair/Biomechanical Upgrades")]
public class BiomechanicalUpgrades : AttachmentCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.Surge(player);

        Card = card;

        List<ICharacter> attachable = new() { owner };
        attachable.AddRange(VillainTurnController.instance.MinionsInPlay);
        attachable.RemoveAll(x => x.Attachments.Where(x => (x as MonoBehaviour).gameObject.name == _card.CardName).Count() != 0);

        int highHP = int.MinValue;

        foreach (var c in attachable)
        {
            if (c.CharStats.Health.BaseHP > highHP)
            {
                highHP = c.CharStats.Health.BaseHP;
                attached = c;
            }
        }

        if (attached != null)
        {
            Attach();
        }
        else
        {
            ScenarioManager.inst.Surge(player);
            ScenarioManager.inst.EncounterDeck.Discard(card);
        }

        return Task.CompletedTask;
    }

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);
        attached.CharStats.Health.Defeated.Add(Defeated);

        _card.transform.SetParent((attached as MonoBehaviour).transform, false);
        _card.transform.SetAsFirstSibling();
        _card.transform.localPosition = new Vector3(-30, 0, 0);

    }

    private void Defeated()
    {
        attached.CharStats.Health.CurrentHealth = attached.CharStats.Health.BaseHP;
        WhenRemoved();
    }

    public override void Detach()
    {
        attached.Attachments.Add(Card as IAttachment);
        attached.CharStats.Health.Defeated.Remove(Defeated);
    }

}
