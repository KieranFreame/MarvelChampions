using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario", fileName = "New Scenario")]
public class ScenarioData : ScriptableObject
{
    public VillainData villain;

    public List<string> RequiredModulars;
    public List<string> RecommendedModulars;
}
