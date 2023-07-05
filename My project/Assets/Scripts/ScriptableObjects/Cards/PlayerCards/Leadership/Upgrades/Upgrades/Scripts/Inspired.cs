using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Inspired", menuName = "MarvelChampions/Card Effects/Leadership/Inspired")]
public class Inspired : PlayerCardEffect
{
    List<AllyCard> allies = new();
    AllyCard ally;

    public override bool CanBePlayed()
    {
        allies.Clear();
        allies.AddRange(_owner.CardsInPlay.Allies);
        allies.RemoveAll(x => x.Attachments.FirstOrDefault(x => (x as ICard).CardName == Card.CardName) != default);

        return allies.Count > 0;
    }

    public override async Task OnEnterPlay()
    {
        ally = await TargetSystem.instance.SelectTarget(allies);

        ally.Attachments.Add(Card as IAttachment);

        Card.transform.SetParent(ally.transform, false);
        Card.transform.SetAsFirstSibling();
        Card.transform.localPosition = new Vector3(-30, 0, 0);

        ally.CharStats.Attacker.CurrentAttack++;
        ally.CharStats.Thwarter.CurrentThwart++;
        ally.CharStats.Health.Defeated += OnDefeat;
    }

    private void OnDefeat()
    {
        ally.Attachments.Remove(Card as IAttachment);

        ally.CharStats.Attacker.CurrentAttack--;
        ally.CharStats.Thwarter.CurrentThwart--;

        _owner.CardsInPlay.Permanents.Remove(Card);
        _owner.Deck.Discard(Card);
    }

    public override void OnExitPlay()
    {
        if (ally is null) return;

        ally.Attachments.Remove(Card as IAttachment);

        ally.CharStats.Attacker.CurrentAttack--;
        ally.CharStats.Thwarter.CurrentThwart--;
    }
}
