using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateEffectUI : MonoBehaviour
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

    public void Yes()
    {
        result = 0;
    }
    public void No()
    {
        result = 1;
    }

    public void LoadStrings(string cardName)
    {
        effectText.text = $"Activate {cardName}'s effect?";
    }
}
