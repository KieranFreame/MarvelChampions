using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DropdownSettings : MonoBehaviour
{
    public string fileName;
    [SerializeField] public TMP_Dropdown _dropdown;

    private void Awake()
    {
        foreach (string line in File.ReadAllLines(fileName))
            _dropdown.options.Add(new TMP_Dropdown.OptionData(line));
    }
}
