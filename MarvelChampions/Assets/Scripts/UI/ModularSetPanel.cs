using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ModularSetPanel : MonoBehaviour
{
    public static ModularSetPanel instance;

    [SerializeField] private Transform contentTransform;
    [SerializeField] GameObject modularPrefab;

    [HideInInspector] public List<string> modularSets = new();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ClearModulars()
    {
        for (int i = modularSets.Count - 1; i >= 0; i--)
        {
            if (modularSets[i] == "Standard" || modularSets[i] == "Expert") continue;
            RemoveModular(modularSets[i]);
        }     
    }

    //Add ScenarioData
    public void AddModular(ScenarioData sData)
    {
        if (sData != null)
        {
            ClearModulars();

            foreach (string modularName in sData.RecommendedModulars)
                CreateModularPrefab(modularName);
        }
    }

    //Add Dropdown
    public void AddModular(string modularName)
    {
        if (!modularSets.Contains(modularName))
             CreateModularPrefab(modularName);

        if (modularName == "Standard")
        {
            RemoveModular("Expert");
        }
    }

    public void RemoveModular(string modularName)
    {
        if (modularSets.Contains(modularName))
        {
            modularSets.Remove(modularName);
            ScenarioManager.inst.EncounterSets.Remove(modularName);
            Destroy(contentTransform.Find(modularName).gameObject);
        }
    }

    void CreateModularPrefab(string modularName)
    {
        GameObject modular = Instantiate(modularPrefab, contentTransform);
        modular.name = modularName;
        modular.transform.Find("ModularName").GetComponent<TMP_Text>().text = modularName;
        modularSets.Add(modularName);
        ScenarioManager.inst.EncounterSets.Add(modularName);
    }
}
