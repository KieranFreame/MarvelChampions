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
        Attached = await TargetSystem.instance.SelectTarget(minions);
        Attach();
    }

    public override async Task Resolve()
    {
        await ThwartSystem.Instance.InitiateThwart(new(3, Owner));
        _owner.Deck.Discard(Card);
    }

    public void Attach()
    {
        Attached.Attachments.Add(this);

        _card.transform.SetParent(((MonoBehaviour)Attached).transform, false);
        _card.transform.SetAsFirstSibling();
        _card.transform.localPosition = new Vector3(-50, 0, 0);
    }

    public void WhenRemoved()
    {
        _owner.CardsInPlay.Permanents.Remove(_card);
        Attached.Attachments.Remove(this);

        if (ScenarioManager.inst.ThreatPresent())
            EffectManager.Inst.Resolving.Push(this);
    }
}
