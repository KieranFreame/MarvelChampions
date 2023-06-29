using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    public ICharacter Target { get; set; }

    public AttackAction(int attack, List<TargetType> targets = null, List<Keywords> _keywords = null, dynamic owner = null)
    {
        Owner = owner;
        Value = attack;
        Keywords = _keywords ?? new List<Keywords>();
        Targets = targets;
    }

    public AttackAction(int attack, ICharacter target, List<Keywords> _keywords = null, ICharacter owner = null)
    {
        Owner = owner; 
        Value = attack;
        Target = target;
        Keywords= _keywords ?? new List<Keywords>();
    }
}
