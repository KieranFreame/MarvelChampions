using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class VillainSelectPanel : MonoBehaviour
{
    [SerializeField] string villainTextFile;

    [Header("Prefabs")]
    [SerializeField] GameObject villainBtnPrefab;
    [SerializeField] Transform contentTransform;

    private void Awake()
    {
        foreach (string line in File.ReadAllLines(villainTextFile))
        {
            CreateVillainButton(line);
        }
    }

    void CreateVillainButton(string villainName)
    {
        GameObject villain = Instantiate(villainBtnPrefab, contentTransform);
        villain.name = villainName;
        villain.GetComponentInChildren<TMP_Text>().text = villainName;
    }
}
