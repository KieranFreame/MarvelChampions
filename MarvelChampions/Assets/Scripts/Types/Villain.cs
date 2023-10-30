using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class Villain : MonoBehaviour, ICharacter
{
    //public VillainData data;
    public ObservableCollection<IAttachment> Attachments { get; set; } = new();
    public string VillainName { get; set; }
    public List<string> VillainTraits { get; private set; }
    public VillainEffect VillainEffect { get; private set; }
    public Sprite Art { get; set; }
    public bool CanAttack { get; set; } = true;
    public bool CanScheme { get; set; } = true;
    public VillainStages Stages { get; private set; }
    public CharacterStats CharStats { get; set; }

    private void Awake()
    {
        Stages = new();
    }

    public async void LoadData(VillainData data)
    {
        VillainName = data.villainName;
        VillainTraits = data.villainTraits;
        VillainEffect = data.villainEffect;

        Art = data.villainArt;
        Stages.Stages = data.stages;

        CharStats = new(this);

        VillainEffect.LoadEffect(this);

        GetComponent<VillainUI>().SetUI(this);

        if (Stages.Stage == 1)
            await VillainEffect.StageOneEffect();
        else
            await VillainEffect.StageTwoEffect();
    }

    public async void WhenDefeated()
    {
        switch (ScenarioManager.inst.Difficulty)
        {
            case Difficulty.Standard:
                if (Stages.Stage == 1)
                {
                    Stages.Stage++;
                    await VillainEffect.StageTwoEffect();
                }
                else
                    Debug.Log("YOU WIN!");
                break;
            case Difficulty.Expert:
                if (Stages.Stage == 2)
                {
                    Stages.Stage++;
                    await VillainEffect.StageThreeEffect();
                }  
                else
                    Debug.Log("YOU WIN!");
                break;
        }
    }

    #region Properties
    public int BaseHP
    {
        get { return Stages.BaseHP; }
    }
    public int BaseAttack
    {
        get { return Stages.BaseAttack; }
    }
    public int BaseScheme
    {
        get { return Stages.BaseScheme; }
    }
    #endregion
}
