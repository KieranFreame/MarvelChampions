using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModularEntry : MonoBehaviour
{
    public void DestroySelf()
    {
        string mod = GetComponentInChildren<TMP_Text>().text;
        ModularSetPanel.RemoveModular(mod);
        Destroy(gameObject);
    }
}
