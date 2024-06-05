using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "Neurological Implants", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Neurological Implants")]
public class NeurologicalImplants : EncounterCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        int highHp = int.MinValue;

        foreach (var m in VillainTurnController.instance.MinionsInPlay.Where(x => !x.Attachments.Any(z => (z as ICard).CardName == card.CardName)))
        {
            if (m.CharStats.Health.CurrentHealth > highHp)
            {
                highHp = m.CharStats.Health.CurrentHealth;
                Attached = m;
            }
        }

        if (Attached == null)
        {
            ScenarioManager.inst.Surge(player);
            ScenarioManager.inst.EncounterDeck.Discard(Card);
            return Task.CompletedTask;
        }
            
        Attach();
        return Task.CompletedTask;
    }

    public void Attach()
    {
        Attached.Attachments.Add(this);
        Attached.CharStats.Attacker.CurrentAttack += 2;
        Attached.CharStats.Schemer.CurrentScheme += 2;
        Attached.CharStats.Health.IncreaseMaxHealth(2);

        _card.transform.SetParent((Attached as MonoBehaviour).transform);
        _card.transform.SetAsFirstSibling();
        _card.transform.localPosition = new Vector3(-30 * Attached.Attachments.Count, 0, 0);
    }

    public void Detach()
    {
        Attached.Attachments.Remove(this);
        Attached.CharStats.Attacker.CurrentAttack -= 2;
        Attached.CharStats.Schemer.CurrentScheme -= 2;
        Attached.CharStats.Health.IncreaseMaxHealth(-2);
    }
}
