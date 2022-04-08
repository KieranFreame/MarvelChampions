using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    int _damage;
    private int baseHP;
    public int currHealth;

    [SerializeField]
    private GameEvent takingDamage;

    public void TakeDamage(int damage)
    {
        _damage = damage;
        takingDamage.Raise();

        if (_damage > 0)
        {
            if (GetComponent<IStatus>().tough)
            {
                GetComponent<IStatus>().tough = false;
                return;
            }

            currHealth -= _damage;

            if (currHealth <= 0)
            {
                GetComponent<IDestructable>().WhenDefeated();
            }
        }   
    }

    public void RecoverHealth(int healing)
    {
        currHealth += healing;

        if (currHealth > baseHP)
            currHealth = baseHP;
    }

    public int BaseHP
    {
        get
        {
            return baseHP;
        }
        set
        {
            baseHP = value;
        }
    }

    public int Damage
    {
        get { return _damage; }
    }
}
