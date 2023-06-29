using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCard : PlayerCard, ICharacter, IExhaust
{
    public List<IAttachment> Attachments { get; private set; } = new List<IAttachment>();
    public CharacterStats CharStats { get; set; }

    public int ThwartConsq { get; set; }
    public int AttackConsq { get; set; }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CharStats.Health.Defeated -= WhenDefeated;
    }

    protected void WhenDefeated()
    {
        Effect.OnExitPlay();
        Owner.CardsInPlay.Allies.Remove(this);
        Owner.Deck.Discard(this);
    }

    public override void LoadCardData(PlayerCardData data, Player owner)
    {
        CharStats = new(this, data);
        CharStats.Health.Defeated += WhenDefeated;

        ThwartConsq = (data as AllyCardData).THWConsq;
        AttackConsq = (data as AllyCardData).ATKConsq;

        base.LoadCardData(data, owner);
    }
}
