using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Caught Off Guard", menuName = "MarvelChampions/Card Effects/Standard I/Caught Off Guard")]
public class CaughtOffGuard : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var _player = TurnManager.instance.CurrPlayer;
        List<PlayerCard> playerCards = new();

        playerCards.AddRange(_player.CardsInPlay.Permanents.Where(x => x.CardType == CardType.Upgrade || x.CardType == CardType.Support));

        if (playerCards.Count == 0)
        {
            ScenarioManager.inst.Surge(_player);
            return;
        }
        else if (playerCards.Count == 1)
        {
            _player.CardsInPlay.Permanents.Remove(playerCards[0]);
            _player.Deck.Discard(playerCards[0]);
        }
        else
        {
            Debug.Log("Discard an Upgrade or Support you control");
            PlayerCard p = await TargetSystem.instance.SelectTarget(playerCards);

            if ((p.Data.cardType is CardType.Upgrade || p.Data.cardType is CardType.Support) && p.InPlay)
            {
                _player.CardsInPlay.Permanents.Remove(p);
                _player.Deck.Discard(p);
            }
        } 
    }
}
