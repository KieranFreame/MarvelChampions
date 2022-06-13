using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Text healthText;

    private void Start()
    {
        var identity = transform.parent.GetComponent<Player>().identity.alterEgo;
        healthText.text = identity.baseHP.ToString();
    }

    public void Refresh()
    {
        healthText.text = transform.parent.GetComponent<Health>().currHealth.ToString();
    }
}
