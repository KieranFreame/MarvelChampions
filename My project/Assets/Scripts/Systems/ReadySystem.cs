using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySystem : MonoBehaviour
{
    #region Singleton
    public static ReadySystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    public void InitiateReady(ReadyAction action) => StartCoroutine(Ready(action));

    public IEnumerator Ready(ReadyAction action)
    {
        yield return TargetSystem.instance.GetTarget<IExhaust>(action, exhaust => { exhaust.Ready(); });
    }
}
