using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainAmericaShield : MonoBehaviour
{
    [SerializeField]
    private int retaliateAmount;

    private void OnEnable()
    {
        GetComponent<Hero>().defence++;
        GetComponent<GameEventListener>().enabled = true;
    }

    private void OnDisable()
    {
        GetComponent<Hero>().defence--;
        GetComponent<GameEventListener>().enabled = false;
    }

    public void Retaliate()
    {
        if (!AttackSystem.instance.Keywords.Contains("Ranged"))
            (AttackSystem.instance.Attacker as MonoBehaviour).gameObject.SendMessage("TakeDamage", retaliateAmount, SendMessageOptions.DontRequireReceiver);
    }
}
