using System.Collections;
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

    #region Fields
    public SchemeAction Action { get; private set; }
    public Threat Target { get; private set; }
    #endregion

    public IEnumerator InitiateScheme(SchemeAction action)
    {
        Action = action;
        Target = null;

        if (Action.Owner is Villain || Action.Keywords.Contains(Keywords.Villainous))
        {
            BoostSystem.instance.DealBoostCards();
            yield return StartCoroutine(BoostSystem.instance.FlipCard(boost => { Action.Value += boost; }));
        }

        Target = FindObjectOfType<MainSchemeCard>().GetComponent<Threat>();
        Debug.Log(Action.Owner.name + " is placing " + Action.Value + " threat on the main scheme");
        Target.GainThreat(Action.Value);

        OnActivationComplete?.Invoke();
        OnSchemeComplete?.Invoke(Action);

        yield return null;
    }
}
