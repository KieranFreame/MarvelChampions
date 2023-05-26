using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Captain Marvel's Helmet", menuName = "MarvelChampions/Card Effects/Captain Marvel/Captain Marvel's Helmet")]
public class CaptainMarvelsHelmet : PlayerCardEffect
{
    public override void OnEnterPlay(Player owner, Card card)
    {
        _owner = owner;
        _card = card;
        _owner.Identity.CharStats.Defender.CurrentDefence += (_owner.Identity.IdentityTraits.Contains("Aerial") ? 2 : 1);

        _owner.Identity.IdentityTraits.CollectionChanged += OnTraitsChange;
    }

    private void OnTraitsChange(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems.Contains("Aerial"))
                    _owner.Identity.CharStats.Defender.CurrentDefence += 1;
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems.Contains("Aerial"))
                    _owner.Identity.CharStats.Defender.CurrentDefence -= 1;
                break;
        }
    }

    public override void OnExitPlay()
    {
        _owner.Identity.CharStats.Defender.CurrentDefence -= (_owner.Identity.IdentityTraits.Contains("Aerial") ? 2 : 1);
        _owner.Identity.IdentityTraits.CollectionChanged -= OnTraitsChange;
    }
}
