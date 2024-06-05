using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Spider-Tracer", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Spider-Tracer")]
public class SpiderTracer : PlayerCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    readonly List<MinionCard> minions = new();

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            minions.Clear();

            if (VillainTurnController.instance.MinionsInPlay.Count == 0)
                return false;

            minions.AddRange(VillainTurnController.instance.MinionsInPlay);
            minions.RemoveAll(x => x.Attachments.FirstOrDefault(x => (x as ICard).CardName == _card.CardName) != default);

            if (minions.Count > 0) return true;
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

        _owner.CardsInPlay.Permanents.Remove(_card);
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
        if (ScenarioManager.inst.ThreatPresent())
            EffectResolutionManager.Instance.ResolvingEffects.Push(this);
    }
}
