using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class SchemeSystem
{
    private static SchemeSystem instance;

    public static SchemeSystem Instance
    {
        get
        {
            if (instance == null)
                instance = new();

            return instance;
        }
    }

    public delegate Task OnSchemeComplete(SchemeAction schemeAction);
    public List<OnSchemeComplete> SchemeComplete { get; private set; } = new();

    #region Modifiers
    public delegate Task<SchemeAction> ModifyThreat(SchemeAction action);
    public List<ModifyThreat> Modifiers { get; private set; } = new();
    #endregion

    #region Fields
    public SchemeAction Action { get; private set; }
    public Threat Target { get; private set; }
    #endregion

    public async Task InitiateScheme(SchemeAction action)
    {
        Action = action;
        Target = null;

        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
        {
            BoostSystem.Instance.DealBoostCards();
            Action.Value += await BoostSystem.Instance.FlipCard(Action);
        }

        Target = ScenarioManager.inst.MainScheme.Threat;

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

        Debug.Log((Action.Owner as MonoBehaviour).gameObject.name + " is placing " + Action.Value + " threat on the main scheme");
        Target.GainThreat(Action.Value);

        for (int i = SchemeComplete.Count -1; i >= 0; --i)
        {
            await SchemeComplete[i](Action);
        }
    }
}
