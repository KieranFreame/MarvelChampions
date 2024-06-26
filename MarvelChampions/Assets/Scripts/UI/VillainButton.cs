using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VillainButton : MonoBehaviour
{
    public void SelectScenario()
    {
        var obj = FindObjectOfType<ScenarioSettings>();

        if (obj != null)
        {
            obj.ChangeScenario(GetComponentInChildren<TMP_Text>().text);
        }
    }
}
