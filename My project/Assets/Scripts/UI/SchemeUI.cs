using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchemeUI : CardUI
{
    public Image CardIcon;
    public Text startingThreatText;
    public Text currentThreatText;

    protected override void Start()
    {
        base.Start();

        startingThreatText.text = (card._cd as SchemeData).startingThreat.ToString();
    }

    private void Update()
    {
        currentThreatText.text = (card as IScheme).threat.currThreat.ToString();
    }
}
