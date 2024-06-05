using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ModularSetButton : MonoBehaviour, IPointerEnterHandler
{
    string _modularName;
    List<string> modularIds = new List<string>();
    Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponentInChildren<Toggle>();
    }

    public void LoadData(string modularName)
    {
        _toggle.GetComponentInChildren<TMP_Text>().text = _modularName = modularName;

        foreach (string line in File.ReadAllLines("Assets/CardLists/Modulars/" + string.Concat(modularName.Where(c => !char.IsWhiteSpace(c))) + ".txt"))
        {
            modularIds.Add(line);
        }

        _toggle.isOn = ScenarioManager.inst.EncounterSets.Contains(modularName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ModularSelectionPanel.instance.LoadCardLabels(modularIds);
    }

    public void ToggleModular()
    {
        if (_toggle.isOn)
            ModularSetPanel.instance.AddModular(_modularName);
        else
            ModularSetPanel.instance.RemoveModular(_modularName);
    }
}
