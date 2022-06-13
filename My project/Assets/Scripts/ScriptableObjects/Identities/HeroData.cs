using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Identity/Hero")]
public class HeroData : ScriptableObject
{
    [Header("CardDetails")]
    public string heroName;
    public List<string> heroTags;

    [Header("Stats")]
    public int baseATK;
    public int baseTHW;
    public int baseDEF;
    public int baseHandSize;

    [Header("Ability")]
    public List<ActionData> actions;
}
