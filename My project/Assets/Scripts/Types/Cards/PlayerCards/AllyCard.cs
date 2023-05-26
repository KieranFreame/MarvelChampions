using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCard : PlayerCard, IAttached, ICharacter, IExhaust
{
    public List<IAttachment> Attachments { get; private set; } = new List<IAttachment>();
    public CharacterStats CharStats { get; set; }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CharStats.Health.Defeated -= WhenDefeated;
    }
    protected virtual void WhenDefeated()
    {
        Effect.OnExitPlay();
        Owner.GetComponent<Player>().CardsInPlay.Allies.Remove(this);
        Owner.GetComponent<Player>().Deck.Discard(this);
    }
    public override void LoadCardData(CardData data, GameObject owner)
    {
        CharStats = new(this, data);
        CharStats.Health.Defeated += WhenDefeated;
        base.LoadCardData(data, owner);
    }
}
