using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Inspired", menuName = "MarvelChampions/Card Effects/Leadership/Inspired")]
public class Inspired : PlayerCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    List<AllyCard> allies = new();

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            allies = _owner.CardsInPlay.Allies.ToList();
            allies.RemoveAll(x => x.Attachments.Any(x => (x as IEffect).Card.CardName == "Inspired"));

            return allies.Count > 0;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        Attached = await TargetSystem.instance.SelectTarget(allies);
        Attached.Attachments.Add(this);

        Attach();
    }

    public void Attach()
    {
        _card.transform.SetParent(((MonoBehaviour)Attached).transform, false);
        _card.transform.SetAsFirstSibling();
        _card.transform.localPosition = new Vector3(-30, 0, 0);

        Attached.CharStats.Attacker.CurrentAttack++;
        Attached.CharStats.Thwarter.CurrentThwart++;
    }

    public void WhenRemoved()
    {
        Detach();

        Attached.Attachments.Remove(this);
        _owner.CardsInPlay.Permanents.Remove(_card);
        _owner.Deck.Discard(Card);
    }

    public void Detach()
    {
        Attached.CharStats.Attacker.BaseATK--;
        Attached.CharStats.Thwarter.BaseThwart--;
    }
}
