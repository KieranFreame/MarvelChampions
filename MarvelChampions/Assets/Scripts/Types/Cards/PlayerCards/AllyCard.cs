using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class AllyCard : PlayerCard, ICharacter, IExhaust
{
    public string Name { get => CardName; }
    public ObservableCollection<IAttachment> Attachments { get; set; } = new();
    public CharacterStats CharStats { get; set; }
    public int ThwartConsq { get; set; }
    public int AttackConsq { get; set; }
    public bool CanAttack { get; set; } = true;
    public bool CanThwart { get; set; } = true;
    public bool CanDefend { get; set; } = true;

    public async void WhenDefeated()
    {
        await Effect.WhenDefeated();

        for (int i = Attachments.Count - 1; i >= 0; i--)
        {
            Attachments[i].WhenRemoved();
        }

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

    public override void Ready()
    {
        if (Exhausted)
        {
            if (TryGetComponent(out _animator))
                _animator.Play("Ready");
            Exhausted = false;

            CanAttack = true;
            CanThwart = true;
            CanDefend = true;
        }
    }

    public override void Exhaust()
    {
        if (!Exhausted)
        {
            if (TryGetComponent(out _animator))
                _animator.Play("Exhaust");
            Exhausted = true;

            CanAttack = false;
            CanThwart = false;
            CanDefend = false;
        }
    }
}
