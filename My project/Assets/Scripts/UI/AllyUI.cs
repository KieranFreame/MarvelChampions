using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyUI : PlayerCardUI
{
    public Text cardThwText;
    public Text cardAtkText;
    public Text cardHPText;

    // Start is called before the first frame update
    protected override void Start()
    {
        if (card.data != null)
            StartCoroutine(LoadData());
    }

    protected override IEnumerator LoadData()
    {
        yield return StartCoroutine(base.LoadData());
        cardThwText.text = (card.data as Ally).thwarter.baseThwart.ToString();
        cardAtkText.text = (card.data as Ally).combatant.baseAttack.ToString();
        cardHPText.text = (card.data as Ally).baseHp.ToString();
    }

    /*public override void Refresh()
    {
        base.Refresh();
        cardThwText.text = (card as Ally).thwart.ToString();
        cardAtkText.text = (card as Ally).attack.ToString();
        cardHPText.text = (card as Ally).hitpoints.currHealth.ToString();
    }*/
}
