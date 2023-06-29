using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Villain : MonoBehaviour, ICharacter
{
    public VillainData data;
    public int Stage { get; private set; }
    public string VillainName { get; private set; }
    public Sprite Art { get; private set; }

    [SerializeField] private List<VillainStage> _stages = new();

    public event UnityAction<int> StageAdvanced;
    public CharacterStats CharStats { get; set; }

    private void Start()
    {
        Stage = 1;
        LoadData(data);

        gameObject.name = VillainName;
    }

    private void LoadData(VillainData data)
    {
        VillainName = data.villainName;
        Art = data.villainArt;
        _stages.AddRange(data.stages);

        CharStats = new(this);
        CharStats.Health.Defeated += WhenDefeated;

        GetComponent<VillainUI>().SetUI(this);
    }

    private void WhenDefeated()
    {
        switch (ScenarioManager.inst.scenario.difficulty)
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

        p.EncounterCards.AddCard(ScenarioManager.inst.EncounterDeck.DealCard());
    }

    #region Properties
    public int BaseHP
    {
        get { return _stages[Stage-1].baseHitpoints; }
    }
    public int BaseAttack
    {
        get { return _stages[Stage -1].baseAttack; }
    }
    public int BaseScheme
    {
        get { return _stages[Stage -1].baseScheme; }
    }
    #endregion
}
