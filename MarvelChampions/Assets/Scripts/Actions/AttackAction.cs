using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    public ICharacter Target { get; set; }
    public ICard Card { get; set; }

    public AttackAction(int attack, List<TargetType> targets, List<string> _keywords = null, dynamic owner = null, ICard card = null)
    {
        Owner = owner;
        Value = attack;
        Keywords = _keywords == null ? new() : _keywords;
        Targets = targets;
    }

    public AttackAction(int attack, ICharacter target, List<string> _keywords = null, ICharacter owner = null, ICard card = null)
    {
        Owner = owner; 
        Value = attack;
        Target = target;
        Keywords= _keywords == null ? new() : _keywords;
        Card = card;
    }

}
