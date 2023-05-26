using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        scenario.difficulty = Difficulty.Standard;
;   }

    public Scenario scenario = new();

    public static ObservableCollection<SchemeCard> sideSchemes = new ObservableCollection<SchemeCard>();
}
