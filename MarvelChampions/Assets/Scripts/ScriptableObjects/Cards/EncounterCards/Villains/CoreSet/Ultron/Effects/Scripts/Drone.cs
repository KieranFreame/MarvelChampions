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

    public void HijackCard(PlayerCardData data, Player owner)
    {
        hijackedCard = data;
        cardOwner = owner;
    }

    public override Task WhenDefeated()
    {
        cardOwner.Deck.discardPile.Add(hijackedCard);
        Destroy(_card.gameObject);

        return Task.CompletedTask;
    }
}
