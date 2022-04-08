using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ally", menuName = "Ally", order = 54)]
public class AllyData : CardData
{
    [SerializeField]
    public int cardCost;
    [SerializeField]
    public int baseHp;
    [SerializeField]
    public int baseAttack;
    [SerializeField]
    public int atkConsq;
    [SerializeField]
    public int baseThwart;
    [SerializeField]
    public int thwConsq;
    [SerializeField]
    public bool ready;
}
