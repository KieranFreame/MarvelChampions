using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "AlphaFlightStation", menuName = "MarvelChampions/Card Effects/Captain Marvel/Alpha Flight Station")]
public class AlphaFlightStation : PlayerCardEffect
{
    public override bool CanActivate()
    {
        return !_card.Exhausted && _owner.Hand.cards.Count >= 1;
    }

    public override async Task Activate()
    {
        _card.Exhaust();

        Debug.Log("Discard a card");
        PlayerCard discard = await TargetSystem.instance.SelectTarget(_owner.Hand.cards.ToList());
        _owner.Deck.Discard(discard);

        DrawCardSystem.Instance.DrawCards(new(amount: (_owner.Identity.ActiveIdentity is AlterEgo) ? 2 : 1));
    }
}
