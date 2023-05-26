using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Caught Off Guard", menuName = "MarvelChampions/Card Effects/Standard I/Caught Off Guard")]
public class CaughtOffGuard : EncounterCardEffect
{
    Player player;

    public override void OnEnterPlay(Villain owner, Card card)
    {
        _owner = owner;
        _card = card;

        player = FindObjectOfType<Player>();

        if (player.CardsInPlay.Permanents.Count == 0)
            _owner.Surge(player);
        else
            _card.StartCoroutine(DiscardUpgradeOrSupport());
        
    }

    private IEnumerator DiscardUpgradeOrSupport()
    {
        List<PlayerCard> playerCards = new();

        playerCards.AddRange(player.CardsInPlay.Permanents);

        yield return _card.StartCoroutine(TargetSystem.instance.SelectTarget(playerCards, playerCard =>
        {
            if (playerCard.CardType is CardType.Upgrade || playerCard.CardType is CardType.Support && playerCard.InPlay)
            {
                player.CardsInPlay.Permanents.Remove(playerCard);
                player.Deck.Discard(playerCard);
            }
        }));
    }
}
