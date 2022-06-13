using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public int result { get; private set; } = -1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    [SerializeField]
    GameObject effectSelection;
    [SerializeField]
    GameObject activateEffect;

    public IEnumerator GetPlayerChoice(List<Action> effects)
    {
        result = -1;

        effectSelection.SetActive(true);
        var ui = effectSelection.GetComponent<EffectSelectionUI>();

        ui.LoadStrings(effects);

        yield return StartCoroutine(ui.GetChoice());
        result = ui.result;

        effectSelection.SetActive(false);
    }

    public IEnumerator ActivateEffect(string cardName)
    {
        result = -1;

        activateEffect.SetActive(true);
        var ui = activateEffect.GetComponent<ActivateEffectUI>();

        ui.LoadStrings(cardName);

        yield return StartCoroutine(ui.GetChoice());
        
        result = ui.result;
        activateEffect.SetActive(false);
    }
}
