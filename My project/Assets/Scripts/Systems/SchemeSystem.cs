using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    #region Events
    public static event UnityAction<Action> OnSchemeComplete;
    public static event UnityAction OnActivationComplete;
    #endregion

    #region Modifiers
    public List<IModifyThreat> Modifiers { get; private set; } = new();
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
            BoostSystem.instance.DealBoostCards();
            Action.Value += await BoostSystem.instance.FlipCard(Action);
        }

        Target = FindObjectOfType<MainSchemeCard>().GetComponent<Threat>();

        for (int i = Modifiers.Count - 1; i >= 0; i--)
        {
            if (Modifiers[i] == null)
            {
                Modifiers.RemoveAt(i);
                continue;
            }

            action = await Modifiers[i].ModifyScheme(action);
            if (action.Value < 0) action.Value = 0;
        }

        Debug.Log((Action.Owner as MonoBehaviour).gameObject.name + " is placing " + Action.Value + " threat on the main scheme");
        Target.GainThreat(Action.Value);

        OnActivationComplete?.Invoke();
        OnSchemeComplete?.Invoke(Action);
    }
}
