using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SchemeSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static SchemeSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    public void InitiateScheme(SchemeAction scheme) => StartCoroutine(Scheme(scheme));

    public IEnumerator Scheme(SchemeAction scheme)
    {
        yield return StartCoroutine(TargetSystem.instance.GetTarget("Scheme", "Threat"));
        var target = TargetSystem.instance.target;

        target.AddThreat(scheme.scheme);

        yield return null;
    }
}
