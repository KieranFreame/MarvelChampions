using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "AlphaFlightStation", menuName = "MarvelChampions/Card Effects/Captain Marvel/Alpha Flight Station")]
public class AlphaFlightStation : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player owner, PlayerCard card)
    {
        _owner = owner;
        Card = card;

        await Task.Yield();
    }

    public override bool CanActivate()
    {
        return !Card.Exhausted;
    }

    public override async Task Activate()
    {
        Card.Exhaust();
        DrawCardSystem.instance.DrawCards(new(amount: (_owner.Identity.ActiveIdentity is AlterEgo) ? 2 : 1));
        await Task.Yield();
    }
}
