using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishButton : MonoBehaviour
{
    private static FinishButton inst;

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

    private GameObject finishBtn;

    public static void ToggleFinishButton(bool toggle, FinishAction func)
    {
        inst.finishBtn.SetActive(toggle);

        if (toggle) OnFinishAction += func;
        else OnFinishAction -= func;
    }

    public void Finish()
    {
        OnFinishAction?.Invoke();
    }
}
