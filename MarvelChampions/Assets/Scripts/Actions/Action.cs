using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Action
{
    public ICharacter Owner { get; protected set; }
    public List<Keywords> Keywords { get; protected set; } = new List<Keywords>();
    public int Value { get; set; }
    public List<TargetType> Targets { get; protected set; } = new List<TargetType>();
    public string Requirement { get; protected set; } = string.Empty;
}
