using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionUI : CardUI
{
    public Text cardThwSchText;
    public Text cardAtkText;
    public Text cardHPText;

    protected override void Start()
    {
        base.Start();

        cardThwSchText.text = (card.data as MinionData).baseScheme.ToString();
        cardAtkText.text = (card.data as MinionData).combatant.baseAttack.ToString();
        cardHPText.text = (card.data as MinionData).baseHealth.ToString();
    }

    public override void Refresh()
    {
        base.Refresh();
        
        //cardThwSchText.text = GetComponent<Schemer>()._Scheme.ToString();
        //cardAtkText.text = GetComponent<Attacker>()._Attack.ToString();
        cardHPText.text = GetComponent<Health>().currHealth.ToString();
        
    }
}
