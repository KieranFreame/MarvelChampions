using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mimicry", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Mimicry")]
public class Mimicry : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is Hero)
        {
            List<PlayerCardData> cards = player.Deck.GetTop(5).Cast<PlayerCardData>().ToList();
            
            if (cards.Any(x => x.cardTraits.Contains("Attack")))
            {
                await owner.CharStats.InitiateAttack();
            }

            player.Deck.Mill(5);
        }
        else
        {
            List<PlayerCardData> cards = player.Deck.GetTop(5).Cast<PlayerCardData>().ToList();

            if (cards.Any(x => x.cardTraits.Contains("Thwart")))
            {
                await owner.CharStats.InitiateScheme();
            }

            player.Deck.Mill(5);
        }
    }
}
