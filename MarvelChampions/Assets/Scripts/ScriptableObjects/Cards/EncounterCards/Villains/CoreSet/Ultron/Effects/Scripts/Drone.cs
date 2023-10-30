using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Drone Token", menuName = "MarvelChampions/Card Effects/Ultron/Drone Token")]
public class Drone : EncounterCardEffect
{
    Player cardOwner;
    PlayerCardData hijackedCard;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        return Task.CompletedTask;
    }

    public void HijackCard(PlayerCardData data, Player owner)
    {
        hijackedCard = data;
        cardOwner = owner;
    }

    public override Task WhenDefeated()
    {
        cardOwner.Deck.discardPile.Add(hijackedCard);
        Destroy(Card.gameObject);

        return Task.CompletedTask;
    }
}
