using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "IronMan", menuName = "MarvelChampions/Identity Effects/Iron Man/Hero")]
    public class IronMan : IdentityEffect
    {
        public override void LoadEffect(Player _owner)
        {
            owner = _owner;
            owner.CardsInPlay.Permanents.CollectionChanged += UpdateHandSize;
        }

        private void UpdateHandSize(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        PlayerCard card = item as PlayerCard;
                        if (card.Data.cardTraits.Contains("Tech") && card.Data.cardType is CardType.Upgrade)
                        {
                            owner.Identity.Hero.HandSize++;

                            if (owner.Identity.Hero.HandSize > 7) { owner.Identity.Hero.HandSize = 7; }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        PlayerCard card = item as PlayerCard;
                        if (card.Data.cardTraits.Contains("Tech") && card.Data.cardType is CardType.Upgrade)
                        {
                            owner.Identity.Hero.HandSize--;
                            if (owner.Identity.Hero.HandSize < 1) { owner.Identity.Hero.HandSize = 1; }
                        }
                    }
                    break;
            }
        }
    }
}

