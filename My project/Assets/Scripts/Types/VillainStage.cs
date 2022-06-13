using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillainStage
{
    public int baseScheme;
    public int baseAttack;
    public int baseHitpoints;

    public List<ActionData> data;

    [Header("Keywords")]
    public bool steady;
    public bool stalwart;
}
