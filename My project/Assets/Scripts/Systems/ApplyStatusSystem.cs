using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ApplyStatusSystem : MonoBehaviour
{
    public static ApplyStatusSystem instance;
    private ApplyStatusAction _action;

    private readonly Dictionary<Status, Type> targetComponents = new() {
        {Status.Stunned, typeof(Attacker) }, 
        {Status.Tough, typeof(Health) }, 
        {Status.Confused, typeof(IConfusable) } 
    };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public IEnumerator ApplyStatus(ApplyStatusAction action)
    {
        _action = action;

        if (_action.Targets.Contains(TargetType.TargetSelf)) //only apply to allies + minions
        {
            switch (targetComponents[_action.Status].GetType().Name)
            {
                case nameof(Attacker):
                    _action.Owner.CharStats.Attacker.Stunned = true;
                    yield break;
                case nameof(Health):
                    _action.Owner.CharStats.Health.Tough = true;
                    yield break;
                case nameof(IConfusable):
                    _action.Owner.CharStats.Thwarter.Confused = true;
                    yield break;
            }
        }
        else
        {
            yield return StartCoroutine(ApplyStatusEffect(targetComponents[_action.Status]));
        }
    }

    private IEnumerator ApplyStatusEffect(Type targetComp)
    {
        List<ICharacter> targets = new();

        foreach (TargetType t in _action.Targets)
        {
            switch (t)
            {
                case TargetType.TargetHero:
                case TargetType.TargetAlterEgo:
                    targets.Add(FindObjectOfType<Player>());
                    break;
                case TargetType.TargetVillain:
                    targets.Add(FindObjectOfType<Villain>());
                    break;
                case TargetType.TargetMinion:
                    targets.AddRange(FindObjectsOfType<MinionCard>());
                    break;
                case TargetType.TargetAlly:
                    targets.AddRange(FindObjectsOfType<AllyCard>());
                    break;
            }
        }

        switch (targetComp.Name)
        {
            case nameof(Attacker):
                if (targets.Count == 1)
                    targets[0].CharStats.Attacker.Stunned = true;
                else
                    yield return StartCoroutine(TargetSystem.instance.SelectTarget(targets, target => {target.CharStats.Attacker.Stunned = true;}));
                break;
            case nameof(Health):
                if (targets.Count == 1)
                    targets[0].CharStats.Health.Tough = true;
                else
                    yield return StartCoroutine(TargetSystem.instance.SelectTarget(targets, target => { target.CharStats.Health.Tough = true; }));
                break;
            case nameof(IConfusable):
                if (targets.Count == 1)
                    targets[0].CharStats.Confusable.Confused = true;
                else
                    yield return StartCoroutine(TargetSystem.instance.SelectTarget(targets, target => { target.CharStats.Confusable.Confused = true; }));
                break;
        }
    }
}
