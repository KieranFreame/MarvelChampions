using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCard : PlayerCard, IAttached, ICharacter, IExhaust
{
    public List<IAttachment> Attachments { get; private set; } = new List<IAttachment>();
    public CharacterStats CharStats { get; set; }

    public int thwartConsq { get; set; }
    public int attackConsq { get; set; }

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

    public void Attack() => StartCoroutine(CharStats.InitiateAttack());
    public void Thwart() => StartCoroutine(CharStats.InitiateThwart());

    public override void LoadCardData(CardData data, GameObject owner)
    {
        CharStats = new(this, data);
        CharStats.Health.Defeated += WhenDefeated;

        thwartConsq = (data as AllyCardData).THWConsq;
        attackConsq = (data as AllyCardData).ATKConsq;

        base.LoadCardData(data, owner);
    }
}
