using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ModularEntry : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ScenarioManager.inst.EncounterSets.Remove(string.Concat(GetComponentInChildren<TMP_Text>().text.Where(c => !char.IsWhiteSpace(c))));
    }
}
