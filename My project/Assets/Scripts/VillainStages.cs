using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class VillainStages
{
    private int _stage;
    public int Stage { 
        get => _stage;
        set
        {
            _stage = value;
            StageAdvanced?.Invoke();
        }
    }
    public List<VillainStage> Stages { get; set; } = new();
    public event UnityAction StageAdvanced;

    public VillainStages()
    {
        Stage = ScenarioManager.inst.Difficulty == Difficulty.Standard ? 1 : 2;
    }

    public int BaseHP
    {
        get { return Stages[Stage - 1].baseHitpoints; }
    }
    public int BaseAttack
    {
        get { return Stages[Stage - 1].baseAttack; }
    }
    public int BaseScheme
    {
        get { return Stages[Stage - 1].baseScheme; }
    }
}
