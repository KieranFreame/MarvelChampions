using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Villain : MonoBehaviour, ICharacter
{
    public VillainData data;

    private int _stage;
    private string _villainName;

    [SerializeField] private List<VillainStage> _stages = new();
    public Deck EncounterDeck;

    private readonly List<Keywords> _keywords = new();

    public event UnityAction SetupComplete;
    public event UnityAction<int> StageAdvanced;

    public CharacterStats CharStats { get; set; }

    private void Start()
    {
        EncounterDeck = new();
        _stage = 1;
        LoadData(data);

        gameObject.name = _villainName;
    }

    private void LoadData(VillainData data)
    {
        _villainName = data.villainName;
        _stages.AddRange(data.stages);

        foreach (string id in Database.GetCardSetByName(_villainName).cardIDs)
            EncounterDeck.AddToDeck(Database.GetCardDataById(id));

        //temp
        foreach (string id in Database.GetCardSetByName("Standard").cardIDs)
            EncounterDeck.AddToDeck(Database.GetCardDataById(id));

        //temp
        foreach (string id in Database.GetCardSetByName("Bomb Scare").cardIDs)
            EncounterDeck.AddToDeck(Database.GetCardDataById(id));

        CharStats = new(this);
        CharStats.Health.Defeated += WhenDefeated;

        SetupComplete?.Invoke();
    }

    private void WhenDefeated()
    {
        switch (ScenarioManager.instance.scenario.difficulty)
        {
            case Difficulty.Standard:
                if (Stage == 1) AdvanceStage();
                else Debug.Log("YOU WIN!");
                break;
            case Difficulty.Expert:
                if (Stage == 2) AdvanceStage();
                else Debug.Log("YOU WIN!");
                break;
        }
    }

    private void AdvanceStage()
    {
        Stage++;
        StageAdvanced?.Invoke(Stage - 1);
    }

    public void Surge(Player p)
    {
        Debug.Log("Surging");

        p.EncounterCards.AddCard(EncounterDeck.deck[0]);
        EncounterDeck.Deal();
    }

    #region Properties
    public int BaseHP
    {
        get { return _stages[_stage-1].baseHitpoints; }
    }
    public int BaseAttack
    {
        get { return _stages[_stage-1].baseAttack; }
    }
    public int BaseScheme
    {
        get { return _stages[_stage-1].baseScheme; }
    }
    public List<Keywords> Keywords
    {
        get => _keywords;
    }
    public int Stage
    {
        get => _stage;
        set => _stage = value;
    }
    public string VillainName
    {
        get => _villainName;
    }
    #endregion
}
