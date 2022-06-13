using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThwartSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static ThwartSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    #region GameEvents
    [SerializeField]
    private GameEvent thwartInitiated;
    [SerializeField]
    private GameEvent thwartComplete;
    #endregion

    public void InitiateThwart(ThwartAction action) => StartCoroutine(Thwart(action));

    public IEnumerator Thwart(ThwartAction action)
    {
        yield return StartCoroutine(TargetSystem.instance.GetTarget("Scheme", "Threat"));
        var target = TargetSystem.instance.target;

        target.RemoveThreat(action.value);

        thwartComplete.Raise();

        yield return null;
    }
}
