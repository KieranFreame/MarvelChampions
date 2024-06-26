using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Spider-Tracer", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Spider-Tracer")]
public class SpiderTracer : PlayerCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    List<MinionCard> minions = new();

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            minions = VillainTurnController.instance.MinionsInPlay.Where(x => !x.Attachments.Any(x => (x as IEffect).Card.CardName == "Spider-Tracer")).ToList();
            return minions.Count > 0;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        Debug.Log(_card); Debug.Log(Card);
        Attached = await TargetSystem.instance.SelectTarget(minions);
        Attach();
    }

    public override async Task Resolve()
    {
        if (ScenarioManager.inst.ThreatPresent())
        {
            Debug.Log("Spider-Tracer has triggered. Select a scheme to remove 3 threat from");
            var schemes = new List<SchemeCard>(ScenarioManager.sideSchemes) { ScenarioManager.inst.MainScheme };
            var target = await TargetSystem.instance.SelectTarget(schemes);
            target.Threat.RemoveThreat(3);
        }

        Detach();
        _owner.Deck.Discard(_card);
    }

    public void Attach()
    {
        GameStateManager.Instance.OnCharacterDefeated += CanTrigger;

        _card.transform.SetParent(((MinionCard)Attached).transform.parent, false);
        _card.transform.SetAsFirstSibling();
        _card.transform.localPosition = new Vector3(-50, 0, 0);
    }

    private void CanTrigger(ICharacter arg0)
    {
        if (arg0 != Attached) return;

        EffectManager.Inst.Resolving.Push(this);
    }

    public void Detach()
    {
        GameStateManager.Instance.OnCharacterDefeated -= CanTrigger;
        _owner.CardsInPlay.Permanents.Remove(_card);
    }
}
