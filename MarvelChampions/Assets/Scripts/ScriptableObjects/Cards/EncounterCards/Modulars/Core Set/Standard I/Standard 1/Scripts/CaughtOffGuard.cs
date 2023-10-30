using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Caught Off Guard", menuName = "MarvelChampions/Card Effects/Standard I/Caught Off Guard")]
public class CaughtOffGuard : EncounterCardEffect
{
    Player _player;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;
        _player = player;

        if (player.CardsInPlay.Permanents.Count == 0)
            ScenarioManager.inst.Surge(player);
        else
            await DiscardUpgradeOrSupport();
        
    }

    private async Task DiscardUpgradeOrSupport()
    {
        List<PlayerCard> playerCards = new();

        playerCards.AddRange(_player.CardsInPlay.Permanents);

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
