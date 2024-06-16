using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    public ICharacter Target { get; set; }
    public ICard Card { get; set; }
    public AttackType AttackType { get; private set; }

    public AttackAction(int attack, List<TargetType> targets, AttackType attackType, List<Keywords> _keywords = null, dynamic owner = null, ICard card = null)
    {
        Owner = owner;
        Value = attack;
        Keywords = _keywords ?? new();
        Targets = targets;
        AttackType = attackType;
        Card = card;
    }

    public AttackAction(int attack, ICharacter target, AttackType attackType, List<Keywords> _keywords = null, ICharacter owner = null, ICard card = null)
    {
        Owner = owner; 
        Value = attack;
        AttackType = attackType;
        Target = target;
        Keywords= _keywords ?? new();
        Card = card;
    }

}

public enum AttackType
{
    Basic,
    Card,
}
