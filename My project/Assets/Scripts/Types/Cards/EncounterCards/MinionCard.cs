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
        CharStats.Health.Defeated -= OnDefeated;
    }

    private void Start()
    {
        if (Data != null)
            LoadCardData(Data, FindObjectOfType<Villain>().gameObject);
    }

    private void OnDefeated()
    {
        Destroy(gameObject); //temp;
    }

    public override void LoadCardData(CardData _data, GameObject owner)
    {
        MinionCardData data = _data as MinionCardData;
        CharStats = new(this, data);
        CharStats.Health.Defeated += OnDefeated;
        base.LoadCardData(data, owner);
    }
}
