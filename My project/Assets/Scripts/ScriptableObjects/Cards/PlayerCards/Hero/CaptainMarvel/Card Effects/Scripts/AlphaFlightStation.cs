using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "AlphaFlightStation", menuName = "MarvelChampions/Card Effects/Captain Marvel/Alpha Flight Station")]
public class AlphaFlightStation : PlayerCardEffect
{
    public override bool CanActivate()
    {
        return !Card.Exhausted;
    }

    public override async Task Activate()
    {
        Card.Exhaust();
        DrawCardSystem.Instance.DrawCards(new(amount: (_owner.Identity.ActiveIdentity is AlterEgo) ? 2 : 1));
        await Task.Yield();
    }
}
