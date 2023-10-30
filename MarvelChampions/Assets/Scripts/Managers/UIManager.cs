using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    #region Events
    public event UnityAction OnUIEnable;
    public static event UnityAction<List<string>> SelectEffect;
    #endregion

    public static bool MakingSelection { get; set; }

    public void UIEnable()
    {
        OnUIEnable?.Invoke();
    }
    public static void ChooseEffect(List<string> effectDescriptions) => SelectEffect?.Invoke(effectDescriptions);
}
