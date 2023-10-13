using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Electromagnetic Backlash", menuName = "MarvelChampions/Card Effects/Nemesis/Iron Man/Electromagnetic Backlash")]
public class ElectromagneticBacklash : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        foreach (Player p in TurnManager.Players)
        {
            int damage = 0;

            for (int i = 0; i < 5; i++)
            {
                PlayerCardData data = p.Deck.deck[0] as PlayerCardData;

                Debug.Log("Top card is " + data.cardName + ".");

                foreach (Resource r in data.cardResources)
                    if (r == Resource.Energy) damage++;

                p.Deck.Mill(1);
            }

            await DamageSystem.Instance.ApplyDamage(new(p, damage, card:Card));
        }
    }
}
