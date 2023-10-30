using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Pain Inhibitors", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Pain Inhibitors")]
public class PainInhibitors : AttachmentCardEffect
{
    Retaliate _retaliate;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        int highHp = int.MinValue;

        foreach (var m in VillainTurnController.instance.MinionsInPlay.Where(x => !x.Attachments.Any(z => (z as ICard).CardName == card.CardName)))
        {
            if (m.CharStats.Health.CurrentHealth > highHp)
            {
                highHp = m.CharStats.Health.CurrentHealth;
                attached = m;
            }
        }

        if (attached == null)
        {
            ScenarioManager.inst.Surge(player);
            ScenarioManager.inst.EncounterDeck.Discard(Card);
            return Task.CompletedTask;
        }
            
        Attach();
        return Task.CompletedTask;
    }

    public override void Attach()
    {
        attached.Attachments.Add(Card as AttachmentCard);
        _retaliate = new(attached, 1);
        attached.CharStats.Health.IncreaseMaxHealth(2);

        base.Attach();
    }

    public override void Detach()
    {
        attached.Attachments.Remove(Card as AttachmentCard);
        _retaliate.WhenRemoved();
        attached.CharStats.Health.IncreaseMaxHealth(-2);
    }
}
