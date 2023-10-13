using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
public class ThwartSystem// : MonoBehaviour
{
    private static ThwartSystem instance = new();

    public static ThwartSystem Instance
    {
        get => instance;
    }

    #region Events
    public static event UnityAction OnThwartComplete;
    #endregion

    #region Modifiers
    public delegate Task<ThwartAction> ModifyThreat(ThwartAction action);
    public List<ModifyThreat> Modifiers { get; private set; } = new();
    public List<SchemeCard> Crisis { get; set; } = new();
    #endregion

    public async Task InitiateThwart(ThwartAction action)
    {
        List<SchemeCard> targets = new();
        targets.AddRange(ScenarioManager.sideSchemes);

        if (Crisis.Count == 0)
        {
            targets.Add(ScenarioManager.inst.MainScheme);
        }

        targets.RemoveAll(x => x.Threat.CurrentThreat == 0);

        if (targets.Count > 1)
            action.Target = await TargetSystem.instance.SelectTarget(targets);
        else
            action.Target = targets[0];

        for (int i = Modifiers.Count - 1; i >= 0; i--)
        {
            if (Modifiers[i] == null)
            {
                Modifiers.RemoveAt(i);
                continue;
            }

            action = await Modifiers[i](action);
            if (action.Value < 0) action.Value = 0;
        }

        action.Target.Threat.RemoveThreat(action.Value);

        OnThwartComplete?.Invoke();
    }
}
