using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsRunner : Minion, IDestructable
{
    private void Awake()
    {
        hitpoints = GetComponent<Health>();
    }

    void Start()
    {
        //GameManager.instance.scenario.activeMinions.Add(this);
        attack = baseAttack = (_cd as AllyData).baseAttack;
        scheme = baseScheme = (_cd as AllyData).baseThwart;
        hitpoints.currHealth = hitpoints.BaseHP = (_cd as AllyData).baseHp;
    }
}
