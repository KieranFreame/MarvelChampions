using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FinishButton : MonoBehaviour
{
    private CancellationTokenSource cancelTokenSource;
    private static FinishButton inst;
    private GameObject finishBtn;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else
            Destroy(this);

        finishBtn = transform.Find("FinishButton").gameObject;
    }

    public delegate void FinishAction();
    public static FinishAction OnFinishAction;

    public static CancellationToken ToggleFinishButton(bool toggle, FinishAction func)
    {
        inst.finishBtn.SetActive(toggle);

        if (toggle)
        {
            OnFinishAction += func;
            inst.cancelTokenSource = new CancellationTokenSource();
            return inst.cancelTokenSource.Token;
        }
        else 
        { 
            OnFinishAction -= func;
            inst.cancelTokenSource.Dispose();
            return default;
        }
    }

    public void Finish()
    {
        cancelTokenSource.Cancel();
    }
}
