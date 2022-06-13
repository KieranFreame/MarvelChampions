using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectManager : MonoBehaviour
{
    #region Singleton
    public static SubjectManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    public void AttachObserver(Observer _observer)
    {
        switch (_observer.trigger)
        {
            case "OnEnterPlay":
                PlayCardSystem.instance.onEnterPlay.Attach(_observer);
                return;
            default:
                return;
        }
    }

    public void DetachObserver(Observer _observer)
    {
        switch (_observer.trigger)
        {
            case "OnEnterPlay":
                PlayCardSystem.instance.onEnterPlay.Detach(_observer);
                return;
            default:
                return;
        }
    }
}
