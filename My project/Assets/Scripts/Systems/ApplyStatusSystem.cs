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
                    _action.Owner.GetComponent<Attacker>().Stunned = true;
                    yield break;
                case nameof(Health):
                    _action.Owner.GetComponent<Health>().Tough = true;
                    yield break;
                case nameof(IConfusable):
                    _action.Owner.GetComponent<IConfusable>().Confused = true;
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
        switch (targetComp.Name)
        {
            case nameof(Attacker):
                yield return StartCoroutine(TargetSystem.instance.GetTarget<Attacker>(_action, attacker => { attacker.Stunned = true; }));
                break;
            case nameof(Health):
                yield return StartCoroutine(TargetSystem.instance.GetTarget<Health>(_action, health => { health.Tough = true; }));
                break;
            case nameof(IConfusable):
                yield return StartCoroutine(TargetSystem.instance.GetTarget<IConfusable>(_action, confusable => { confusable.Confused = true; }));
                break;
        }
    }
}
