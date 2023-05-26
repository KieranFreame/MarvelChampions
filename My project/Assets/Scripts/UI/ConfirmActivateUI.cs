using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmActivateUI : MonoBehaviour
{
    private static ConfirmActivateUI inst;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        _confirmChoicePanel = transform.Find("ConfirmChoicePanel").gameObject;
        _activateEffectText = _confirmChoicePanel.transform.Find("ActivateText").GetComponent<Text>();
        _effectDescText = _confirmChoicePanel.transform.Find("CardDescText").GetComponent<Text>();
    }

    private GameObject _confirmChoicePanel;
    private Text _activateEffectText;
    private Text _effectDescText;

    private bool choiceMade = false;
    private bool activate;

    public static IEnumerator MakeChoice(Card card, System.Action<bool> callback)
    {
        inst._activateEffectText.text = "Activate " + card.CardName + "?";
        inst._effectDescText.text = card.CardDesc;
        inst.choiceMade = false;
        inst._confirmChoicePanel.SetActive(true);

        while (!inst.choiceMade) { yield return null; }

        inst._confirmChoicePanel.SetActive(false);
        callback(inst.activate);
    }

    public void YesActivate()
    {
        activate = true;
        choiceMade = true;
        
    }

    public void NoActivate()
    {
        activate = false;
        choiceMade = true;
    }
}
