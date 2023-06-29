using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading.Tasks;

public class ChooseEffectUI : MonoBehaviour
{
    private static ChooseEffectUI inst;

    private void Awake()
    {
        inst ??= this;
        if (inst != this) Destroy(this);
    }

    [SerializeField] private GameObject chooseEffectPanel;
    [SerializeField] private Text effectDescText;
    [SerializeField] private Text effectToActivateText;
    [SerializeField] private List<Button> buttonList;

    private bool effectSelected = false;
    private int selectedEffect;
    private List<string> effectDescriptions;

    public static async Task<int> ChooseEffect(List<string> effectDescs)
    {
        inst.effectDescriptions = effectDescs;
        inst.effectSelected = false;

        foreach (Button b in inst.buttonList)
            b.gameObject.SetActive(false);

        for (int i = 0; i < inst.effectDescriptions.Count; i++)
            inst.buttonList[i].gameObject.SetActive(true);

        inst.effectDescText.text = inst.effectDescriptions[0];
        inst.chooseEffectPanel.SetActive(true);

        while (!inst.effectSelected)
            await Task.Yield();

        inst.chooseEffectPanel.SetActive(false);
        return inst.selectedEffect;
    }

    public void ViewEffect(int index)
    {
        selectedEffect = index;
        effectDescText.text = effectDescriptions[index-1];
    }

    public void SelectEffect() => effectSelected = true;
}
