using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CaptainAmericaHeroPack
{
    [CreateAssetMenu(fileName = "Captain America", menuName = "MarvelChampions/Identity Effects/Captain America/Hero")]
    public class CaptainAmerica : IdentityEffect
    {
        public override void LoadEffect(Player _owner)
        {
            owner = _owner;
            hasActivated = false;
            TurnManager.OnStartPlayerPhase += Reset;
        }

        public override bool CanActivate()
        {
            if (!owner.Exhausted)
                return false;

            if (owner.Hand.cards.Count == 0)
                return false;

            return !hasActivated;
        }

        public override async void Activate()
        {
            PlayerCard card = await TargetSystem.instance.SelectTarget(owner.Hand.cards.ToList());

            owner.Hand.Remove(card);
            owner.Deck.Discard(card);

            owner.Ready();
            hasActivated = true;
        }
    }
}

