using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchemeUI : CardUI
{
    public Image CardIcon;
    public Text startingThreatText;
    public Text currentThreatText;
    public Text maxThreatText;

    protected override void Start()
    {
        base.Start();

        startingThreatText.text = (card.data as Scheme).startingThreat.ToString();
        currentThreatText.text = startingThreatText.text;

        if (maxThreatText != null)
            maxThreatText.text = "12P";
    }

    public override void Refresh()
    {
        base.Refresh();

        currentThreatText.transform.parent.gameObject.SetActive(true);

        if (GetComponent<Threat>().currThreat > 0)
            currentThreatText.text = GetComponent<Threat>().currThreat.ToString();
        else
        {
            currentThreatText.transform.parent.gameObject.SetActive(false);
        }
    }
}
