using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class DisplayAllyCard : DisplayPlayerCard
{
    public int Attack { get => (Data as AllyCardData).BaseATK; }
    public int Thwart { get => (Data as AllyCardData).BaseTHW; }
    public int Health { get => (Data as AllyCardData).BaseHP; }
}
