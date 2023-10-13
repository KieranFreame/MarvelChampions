using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class AllyCard : PlayerCard, ICharacter, IExhaust
{
    public ObservableCollection<IAttachment> Attachments { get; set; } = new();
    public CharacterStats CharStats { get; set; }
    public int ThwartConsq { get; set; }
    public int AttackConsq { get; set; }
    public bool CanAttack { get; set; } = true;
    public bool CanThwart { get; set; } = true;

    public async void WhenDefeated()
    {
        await Effect.WhenDefeated();
        Owner.CardsInPlay.Allies.Remove(this);
        Owner.Deck.Discard(this);
    }

    public override void LoadCardData(PlayerCardData data, Player owner)
    {
        CharStats = new(this, data);

        ThwartConsq = (data as AllyCardData).THWConsq;
        AttackConsq = (data as AllyCardData).ATKConsq;

        base.LoadCardData(data, owner);
    }
}
