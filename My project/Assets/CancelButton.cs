using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class CancelButton : MonoBehaviour
{
    private CancellationTokenSource cancelTokenSource;
    private static CancelButton inst;
    private GameObject cancelBtn;

    private void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(this);

        cancelBtn = transform.Find("CancelButton").gameObject;
    }

    public delegate void CancelAction();
    public static CancelAction OnCancelAction;

    public static CancellationToken ToggleCancelBtn(bool toggle, CancelAction func)
    {
        inst.cancelBtn.SetActive(toggle);

        if (toggle)
        {
            //turning button on
            OnCancelAction += func;
            inst.cancelTokenSource = new CancellationTokenSource();
            return inst.cancelTokenSource.Token;
        }
        else
        {
            //turning it off
            OnCancelAction -= func;
            inst.cancelTokenSource.Dispose();
            return default;
        }
    }

    public void Cancel()
    {
        cancelTokenSource.Cancel();
    }
}
