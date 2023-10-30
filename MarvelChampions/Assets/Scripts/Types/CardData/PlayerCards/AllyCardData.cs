using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ally", menuName = "MarvelChampions/Card/Ally")]
public class AllyCardData : PlayerCardData
{
    [Header("Ally Data")]
    public string alterEgo;

    [Header("Attack Stats")]
    public int BaseATK;
    public int ATKConsq;

    [Header("Thwart Stats")]
    public int BaseTHW;
    public int THWConsq;

    [Header("Health")]
    public int BaseHP;
    
}
