using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Counters : MonoBehaviour
{
    public GameObject Counter { get; set; }
    TMP_Text counterText;

    private int countersLeft;

    public int CountersLeft {
        get => countersLeft; 
        set
        {
            countersLeft = value;
            counterText.text = CountersLeft.ToString();
        }
    }

    private void Awake()
    {
        Counter = Instantiate(Resources.Load<GameObject>("Prefabs/Counters"), transform, false);
        counterText = Counter.GetComponentInChildren<TMP_Text>();
        counterText.text = CountersLeft.ToString();
    }

    public void RemoveCounters(int counters)
    {
        CountersLeft -= counters;
        counterText.text = CountersLeft.ToString();
    }

    public void AddCounters(int counters)
    {
        CountersLeft += counters;
        counterText.text = CountersLeft.ToString();
    }
}
