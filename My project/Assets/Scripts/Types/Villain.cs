using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain : MonoBehaviour
{
    public VillainData data;

    public int stage;
    public List<AbilityLoader> actions; //one for each stage;

    private void Start()
    {
        SetStage();

        foreach (VillainStage s in data.stages)
        {
            var loader = LoaderFactory.instance.CreateLoader(new Card(null)); //temp
            loader.AddAction(ActionFactory.instance.CreateAction(s.data[0]));
            actions.Add(loader);
        }
    }

    private void SetStage()
    {
        switch (ScenarioManager.instance.scenario.difficulty)
        {
            case Difficulty.Standard: stage = 1; break;
            case Difficulty.Expert: stage = 2; break;
        }
    }
}
