using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DropdownSettings : MonoBehaviour
{
    public string fileName;
    public string previewText;
    public TMP_Dropdown _dropdown { get; set; }

    private void Awake()
    {
        _dropdown = transform.GetComponentInChildren<TMP_Dropdown>();
        _dropdown.ClearOptions();

        _dropdown.options.Add(new TMP_Dropdown.OptionData(previewText));

        foreach (string line  in File.ReadAllLines(fileName))
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData(line));
        }
    }
}
