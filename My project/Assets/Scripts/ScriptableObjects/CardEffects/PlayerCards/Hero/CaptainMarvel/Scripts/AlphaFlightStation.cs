using UnityEngine;

[CreateAssetMenu(fileName = "AlphaFlightStation", menuName = "MarvelChampions/Card Effects/Captain Marvel/Alpha Flight Station")]
public class AlphaFlightStation : PlayerCardEffect
{
    public override void OnEnterPlay(Player owner, Card card)
    {
        _owner = owner;
        _card = card;
    }

    public override bool CanActivate()
    {
        return !(_card as PlayerCard).Exhausted;
    }

    public override void Activate()
    {
        (_card as PlayerCard).Exhaust();
        DrawCardSystem.instance.DrawCards(new(amount: (_owner.Identity.ActiveIdentity is AlterEgo) ? 2 : 1));
    }
}
