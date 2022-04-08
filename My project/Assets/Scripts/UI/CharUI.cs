using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharUI : CardUI
{
    public Text cardCostText;
    public Text cardThwSchText;
    public Text cardAtkText;
    public Text cardHPText;

    private void Update()
    {
        if (card is Ally)
        {
            cardCostText.text = (card._cd as AllyData).cardCost.ToString();
            cardThwSchText.text = (card as Ally).thwart.ToString();
            cardAtkText.text = (card as Ally).attack.ToString();
            cardHPText.text = (card as Ally).hitpoints.currHealth.ToString();
        }

        if (card is Minion)
        {
            cardCostText.text = (card._cd as AllyData).cardCost.ToString();
            cardThwSchText.text = (card as Minion).scheme.ToString();
            cardAtkText.text = (card as Minion).attack.ToString();
            cardHPText.text = (card as Minion).hitpoints.currHealth.ToString();
        }
    }
}
