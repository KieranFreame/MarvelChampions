using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Biomechanical Upgrades", menuName = "MarvelChampions/Card Effects/The Doomsday Chair/Biomechanical Upgrades")]
public class BiomechanicalUpgrades : AttachmentCardEffect, IAttachment
{
    public ICharacter Attached { get => attached; set => attached = value; }

    public override Task OnEnterPlay()
    {
        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);

        List<ICharacter> attachable = new(VillainTurnController.instance.MinionsInPlay) { _owner };
        attachable.RemoveAll(x => x.Attachments.Any(x => ((IEffect)x).Card.CardName == _card.CardName));

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
            Attach();
        else
            ScenarioManager.inst.EncounterDeck.Discard(_card);

        return Task.CompletedTask;
    }

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);
        GameStateManager.Instance.OnCharacterDefeated += Defeated;

        _card.transform.SetParent((attached as MonoBehaviour).transform, false);
        _card.transform.SetAsFirstSibling();
        _card.transform.localPosition = new Vector3(-30, 0, 0);

    }

    private void Defeated(ICharacter card)
    {
        if (card != attached) return;

        attached.CharStats.Health.CurrentHealth = attached.CharStats.Health.BaseHP;
        WhenRemoved();
    }

    public override void Detach()
    {
        attached.Attachments.Add(this);
        GameStateManager.Instance.OnCharacterDefeated -= Defeated;
    }

}
