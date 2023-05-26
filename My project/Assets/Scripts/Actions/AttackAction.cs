using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    public AttackAction(int attack, TargetType target = TargetType.TargetNone, List<Keywords> _keywords = null, dynamic owner = null)
    {
        Owner = owner;
        Value = attack;
        Keywords = _keywords ?? new List<Keywords>();
        Targets.Add(target);
    }
    
    public AttackAction(int attack, List<TargetType> targets = null, List<Keywords> _keywords = null)
    {
        Value = attack;
        Keywords = _keywords;
        Targets.AddRange(targets);
    }
}
