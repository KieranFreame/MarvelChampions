using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModularSetPanel : MonoBehaviour
{
    DropdownSettings dropdown;
    [SerializeField] private Transform contentTransform;

    [SerializeField] GameObject modularPrefab;
    [SerializeField] GameObject requiredModularPrefab;

    static List<string> addedModulars = new();

    private void Awake()
    {
        dropdown = GetComponent<DropdownSettings>();
    }

    void ClearModulars()
    {
        foreach (Transform child in contentTransform)
            Destroy(child.gameObject);

        addedModulars.Clear();
    }

    //Add ScenarioData
    public void AddModular(ScenarioData sData)
    {
        if (sData == null) { return; }

        ClearModulars();

        foreach (string modularName in sData.RequiredModulars)
        {
            CreateModularPrefab(modularName, true);
        }
        
        foreach (string modularName in sData.RecommendedModulars)
        {
            CreateModularPrefab(modularName);
        }
    }

    //Add Dropdown
    public void AddModular()
    {
        //don't add placeholder
        if (dropdown._dropdown.captionText.text == "Select Modular")
            return;

        //no duplicates
        if (addedModulars.Contains(dropdown._dropdown.captionText.text))
            return;

        CreateModularPrefab(dropdown._dropdown.captionText.text);
    }

    public static void RemoveModular(string modularToRemove)
    {
        addedModulars.Remove(modularToRemove);
    }

    void CreateModularPrefab(string modularName, bool mandatory = false)
    {
        GameObject modular = Instantiate(mandatory ? requiredModularPrefab : modularPrefab, contentTransform);
        modular.transform.Find("ModularName").GetComponent<TMP_Text>().text = modularName;
        addedModulars.Add(modularName);
    }
}
