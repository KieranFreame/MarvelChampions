using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "Defensive Programming", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Defensive Programming")]
public class DefensiveProgramming : EncounterCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    public override Task OnEnterPlay()
    {
        int highHp = int.MinValue;

        foreach (var m in VillainTurnController.instance.MinionsInPlay.Where(x => !x.Attachments.Any(z => ((IEffect)z).Card.CardName == _card.CardName)))
        {
            if (m.CharStats.Health.CurrentHealth > highHp)
            {
                highHp = m.CharStats.Health.CurrentHealth;
                Attached = m;
            }
        }

        if (Attached == null)
        {
            ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
            ScenarioManager.inst.EncounterDeck.Discard(Card);
            return Task.CompletedTask;
        }

        Attach();

        return Task.CompletedTask;
    }

    public void Attach()
    {
        Attached.Attachments.Add(this);
        AttackSystem.Instance.Guards.Add(Attached as MinionCard);
        Attached.CharStats.Health.IncreaseMaxHealth(2);

        _card.transform.SetParent((Attached as MonoBehaviour).transform);
        _card.transform.SetAsFirstSibling();
        _card.transform.localPosition = new Vector3(-30 * Attached.Attachments.Count, 0, 0);
    }

    public void Detach()
    {
        Attached.Attachments.Remove(this);
        AttackSystem.Instance.Guards.Remove(Attached as MinionCard);
        Attached.CharStats.Health.IncreaseMaxHealth(-2);
    }
}
