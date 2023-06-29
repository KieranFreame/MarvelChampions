using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Counters : MonoBehaviour
{
    public GameObject Counter { get; set; }
    public int CountersLeft { get; set; } = 0;

    private void Awake()
    {
        Counter = Instantiate(Resources.Load<GameObject>("Prefabs/Counters"), transform, false);
        Counter.GetComponentInChildren<TMP_Text>().text = CountersLeft.ToString();
    }

    public void RemoveCounters(int counters)
    {
        CountersLeft -= counters;
        Counter.GetComponentInChildren<TMP_Text>().text = CountersLeft.ToString();
    }

    public void AddCounters(int counters)
    {
        CountersLeft += counters;
        Counter.GetComponentInChildren<TMP_Text>().text = CountersLeft.ToString();
    }
}
