using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyActions : PlayerCardActions
{
    private GameObject attackBtn;
    private GameObject thwartBtn;

    protected override void Awake()
    {
        base.Awake();
        attackBtn = transform.Find("Attack").gameObject;
        thwartBtn = transform.Find("Thwart").gameObject;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (UIManager.MakingSelection) return;

        if (card.CurrZone == Zone.Ally)
        {
            attackBtn.SetActive(!card.Exhausted);
            thwartBtn.SetActive(!card.Exhausted);
        }
        else
        {
            attackBtn.SetActive(false);
            thwartBtn.SetActive(false);
        }
    }

    public async void Attack()
    {
        await (card as AllyCard).CharStats.InitiateAttack();
        gameObject.SetActive(false);
    }
    public async void Thwart()
    {
        await (card as AllyCard).CharStats.InitiateThwart();
        gameObject.SetActive(false);
    }
    
}
