using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ThwartSystem : MonoBehaviour
{
    public static ThwartSystem instance;

    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    #region Events
    public static event UnityAction OnThwartComplete;
    #endregion

    #region Fields
    public ThwartAction Action { get; private set; }
    public Threat Target { get; private set; }
    #endregion

    public IEnumerator InitiateThwart(ThwartAction action)
    {
        instance.Action = action;

        List<Threat> targets = new(); 
        targets.AddRange(FindObjectsOfType<Threat>());

        yield return StartCoroutine(TargetSystem.instance.SelectTarget(targets, threat => { Target = threat; }));

        Target.RemoveThreat(Action.Value);

        OnThwartComplete?.Invoke();
    }
}
