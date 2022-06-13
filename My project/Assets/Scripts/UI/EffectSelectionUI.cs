using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSelectionUI : MonoBehaviour
{
    [SerializeField]
    Text effectText;
    List<string> effects = new List<string>();
    public int result = -1;

    public IEnumerator GetChoice()
    {
        int prevResult = result;

        yield return new WaitUntil(() => result != prevResult);

        yield return null;
    }

    public void btnOption1()
    {
        effectText.text = effects[0];
    }
    public void btnOption2()
    {
        effectText.text = effects[1];
    }
    public void btnOption3()
    {
        effectText.text = effects[2];
    }

    public void ConfirmChoice()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i] == effectText.text)
            {
                result = i;
                return;
            }
        }
    }

    public void LoadStrings(List<Action> effects)
    {
        foreach (Action effect in effects)
        {
            /*if (effect != null)
                this.effects.Add(effect.actionDesc);
            //add button to panel;*/
        }
            

        effectText.text = this.effects[0];
    }
}
