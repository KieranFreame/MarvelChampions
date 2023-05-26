using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChooseEffectUI : MonoBehaviour
{
    [SerializeField] private GameObject chooseEffectPanel;
    [SerializeField] private Text effectDescText;
    [SerializeField] private Text effectToActivateText;
    [SerializeField] private List<Button> buttonList;

    private int selectedEffect;
    private List<string> effectDescriptions;

    public static event UnityAction<int> EffectSelected;

    private void OnEnable()
    {
        UIManager.SelectEffect += ChooseEffect;
    }

    private void ChooseEffect(List<string> effectDescs)
    {
        effectDescriptions = effectDescs;

        foreach (Button b in buttonList)
            b.gameObject.SetActive(false);

        for (int i = 0; i < effectDescriptions.Count; i++)
            buttonList[i].gameObject.SetActive(true);

        effectDescText.text = effectDescriptions[0];
        chooseEffectPanel.SetActive(true);
    }

    public void Effect1()
    {
        selectedEffect = 1;
        effectDescText.text = effectDescriptions[0];
    }

    public void Effect2()
    {
        selectedEffect = 2;
        effectDescText.text = effectDescriptions[1];
    }

    public void Effect3()
    {
        selectedEffect = 3;
        effectDescText.text = effectDescriptions[2];
    }

    public void SelectEffect()
    {
        EffectSelected?.Invoke(selectedEffect);
        chooseEffectPanel.SetActive(false);
    }
}
