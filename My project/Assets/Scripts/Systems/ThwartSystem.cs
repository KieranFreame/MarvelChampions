using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async Task InitiateThwart(ThwartAction action)
    {
        instance.Action = action;

        List<Threat> targets = new() { FindObjectOfType<MainSchemeCard>().GetComponent<Threat>() };
        foreach (SchemeCard s in ScenarioManager.sideSchemes)
            targets.Add(s.GetComponent<Threat>());

        if (targets.Count > 1)
            Target = await TargetSystem.instance.SelectTarget(targets);
        else
            Target = targets[0];

        Target.RemoveThreat(Action.Value);

        OnThwartComplete?.Invoke();
    }
}
