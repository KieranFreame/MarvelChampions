using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schemer : MonoBehaviour
{
    public int _scheme { get; set; }
    public List<string> keywords = new List<string>(); //temp?
    public int baseSCH { get; set; }
    public bool confused;

    private void Start()
    {
        var data = GetComponent<CardUI>().card.data as MinionData;
        _scheme = baseSCH = data.baseScheme;
    }

    public void Scheme()
    {

        if (confused)
        {
            confused = false;
            return;
        }

        var scheme = new SchemeAction(owner:this);
        scheme.Execute();
    }
}
