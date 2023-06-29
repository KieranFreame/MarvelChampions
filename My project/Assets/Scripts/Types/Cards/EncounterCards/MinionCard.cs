using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinionCard : EncounterCard, ICharacter
{
    public int BaseAttack { get => (Data as MinionCardData).baseAttack; }
    public int BaseScheme { get => (Data as MinionCardData).baseScheme; }
    public int BaseHP { get => (Data as MinionCardData).baseHealth; }
    public CharacterStats CharStats { get; set; }

    private void OnDisable()
    {
        CharStats.Health.Defeated -= WhenDefeated;
    }

    protected override void WhenDefeated()
    {
        VillainTurnController.instance.MinionsInPlay.Remove(this);
        base.WhenDefeated();
    }
    public override void LoadCardData(EncounterCardData _data, Villain owner)
    {
        MinionCardData data = _data as MinionCardData;
        CharStats = new(this, data);
        CharStats.Health.Defeated += WhenDefeated;
        base.LoadCardData(data, owner);
    }
}
