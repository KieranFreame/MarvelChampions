using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Defensive Programming", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Defensive Programming")]
public class DefensiveProgramming : AttachmentCardEffect
{
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
        AttackSystem.Instance.Guards.Add(attached as MinionCard);
        attached.CharStats.Health.IncreaseMaxHealth(2);

        base.Attach();
    }

    public override void Detach()
    {
        attached.Attachments.Remove(Card as AttachmentCard);
        AttackSystem.Instance.Guards.Remove(attached as MinionCard);
        attached.CharStats.Health.IncreaseMaxHealth(-2);
    }
}
