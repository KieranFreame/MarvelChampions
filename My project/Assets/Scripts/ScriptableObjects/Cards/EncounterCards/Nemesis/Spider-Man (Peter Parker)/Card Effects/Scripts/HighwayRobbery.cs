using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Highway Robbery", menuName = "MarvelChampions/Card Effects/Nemesis/Spider-Man (Peter Parker)/Highway Robbery")]
public class HighwayRobbery : EncounterCardEffect
{
    List<PlayerCard> stolenCards = new();

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        float x = 50;

        foreach (Player p in TurnManager.Players)
        {
            var pCard = p.Hand.cards[Random.Range(0, p.Hand.cards.Count)];
            p.Hand.cards.Remove(pCard);
            stolenCards.Add(pCard);

            pCard.transform.SetParent(card.transform.parent, false);
            pCard.transform.SetAsFirstSibling();
            pCard.transform.localPosition = new Vector3(x, 0, 0);
            x -= 10;
        }

        FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().Acceleration += 1;

        await Task.Yield();
    }

    public override async Task WhenDefeated()
    {
        foreach (Player p in TurnManager.Players)
        {
            var pCard = stolenCards.FirstOrDefault(x => x.Owner == p);

            stolenCards.Remove(pCard);
            p.Hand.cards.Add(pCard);

            pCard.transform.SetParent(GameObject.Find("PlayerHandTransform").transform);
        }

        FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().Acceleration -= 1;
        await Task.Yield();
    }
}
