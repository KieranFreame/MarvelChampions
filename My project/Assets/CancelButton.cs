using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CancelButton : MonoBehaviour
{
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

    public static void ToggleCancelBtn(bool toggle, CancelAction func)
    {
        inst.cancelBtn.SetActive(toggle);

        if (toggle) OnCancelAction += func; //turning button on
        else OnCancelAction -= func; //turning it off
    }

    public void Cancel()
    {
        OnCancelAction?.Invoke();
    }
}
