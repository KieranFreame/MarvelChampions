using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Spider-Tracer", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Spider-Tracer")]
public class SpiderTracer : PlayerCardEffect
{
    readonly List<MinionCard> minions = new();
    MinionCard target;

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            minions.Clear();

            if (VillainTurnController.instance.MinionsInPlay.Count == 0)
                return false;

            minions.AddRange(VillainTurnController.instance.MinionsInPlay);
            minions.RemoveAll(x => x.Attachments.FirstOrDefault(x => (x as ICard).CardName == Card.CardName) != default);

            if (minions.Count > 0) return true;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        target = await TargetSystem.instance.SelectTarget(minions);

        target.Attachments.Add(Card as IAttachment);

        Card.transform.SetParent(target.transform, false);
        Card.transform.SetAsFirstSibling();
        Card.transform.localPosition = new Vector3(-50, 0, 0);

        target.CharStats.Health.Defeated.Add(WhenDefeated);
    }

    public override async Task WhenDefeated()
    {
        target.CharStats.Health.Defeated.Remove(WhenDefeated);

        Card.transform.SetParent(null);

        if (ScenarioManager.sideSchemes.Count == 0 && ScenarioManager.inst.MainScheme.Threat.CurrentThreat == 0)
            return;

        await ThwartSystem.Instance.InitiateThwart(new(3));

        _owner.CardsInPlay.Permanents.Remove(Card);
        _owner.Deck.Discard(Card);
    }
}
