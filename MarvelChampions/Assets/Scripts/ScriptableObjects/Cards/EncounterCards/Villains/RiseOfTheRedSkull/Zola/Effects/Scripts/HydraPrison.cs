using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Prison", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Hydra Prison")]
public class HydraPrison : EncounterCardEffect
{
    Dictionary<Player, AllyCardData> prisoners = new();

    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        foreach (var p in TurnManager.Players)
        {
            AllyCardData prisoner = p.Deck.deck.FirstOrDefault(x => x.cardType == CardType.Ally && (x as PlayerCardData).cardAspect == Aspect.Hero) as AllyCardData;

            if (prisoner == default)
            {
                prisoner = p.Deck.discardPile.FirstOrDefault(x => x.cardType == CardType.Ally && (x as PlayerCardData).cardAspect == Aspect.Hero) as AllyCardData;

                if (prisoner == default)
                {
                    PlayerCard ally = p.Hand.cards.FirstOrDefault(x => x.CardType == CardType.Ally && x.CardAspect == Aspect.Hero);
                    prisoner = ally.Data as AllyCardData;

                    if (prisoner == default)
                        continue; //no ally (Rocket, Groot, Hulk, etc.)
                    else
                    {
                        p.Hand.RemoveFromHand(ally);
                        p.Deck.limbo.Remove(prisoner);
                        Destroy(ally);
                    }
                }
                else
                {
                    p.Deck.discardPile.Remove(prisoner);
                }
            }
            else
            {
                p.Deck.deck.Remove(prisoner);
            }

            prisoners.Add(p, prisoner);
            p.Deck.Shuffle();
        }

        foreach (var prisoner in prisoners.Values)
        {
            (card as SchemeCard).Threat.GainThreat(prisoner.cardCost);
        }

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        foreach (var p  in TurnManager.Players)
        {
            if (prisoners.ContainsKey(p)) //no ally (Hulk, Groot, Rocket, etc.)
            {
                PlayerCardData ally = prisoners[p];
                p.Deck.limbo.Add(ally);
                p.Hand.AddToHand(CreateCardFactory.Instance.CreateCard(ally, GameObject.Find("PlayerHandTransform").transform) as PlayerCard);
            }
        }

        ScenarioManager.inst.RemoveFromGame(_card.Data);
        Destroy(_card.gameObject);

        return Task.CompletedTask;
    }
}
