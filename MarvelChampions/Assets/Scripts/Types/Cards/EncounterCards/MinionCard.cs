using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

public class MinionCard : EncounterCard, ICharacter
{
    public ObservableCollection<IAttachment> Attachments { get; set; } = new();
    public int BaseAttack { get => (Data as MinionCardData).baseAttack; }
    public int BaseScheme { get => (Data as MinionCardData).baseScheme; }
    public int BaseHP { get => (Data as MinionCardData).baseHealth; }
    public bool CanAttack { get; set; } = true;
    public bool CanScheme { get; set; } = true;

    public CharacterStats CharStats { get; set; }

    public async void WhenDefeated()
    {
        await Effect.WhenDefeated();

        for (int i = Attachments.Count - 1; i >= 0; i--)
        {
            (Attachments[i] as AttachmentCard).Effect.WhenRemoved();
        }

        VillainTurnController.instance.MinionsInPlay.Remove(this);
        ScenarioManager.inst.EncounterDeck.Discard(this);
    }

    public override void LoadCardData(EncounterCardData _data, Villain owner)
    {
        MinionCardData data = _data as MinionCardData;
        CharStats = new(this, data);
        base.LoadCardData(data, owner);
    }
}
