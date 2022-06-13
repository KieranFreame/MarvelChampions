using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Health : MonoBehaviour
{
    int _damage;
    private int baseHP;
    public int currHealth;
    public bool tough = false;
    [SerializeField]
    private bool isPlayer;
    private CardData data;

    public Subject takingDamage { get; set; }

    private void Start()
    {
        takingDamage = new Subject(this);

        if (!isPlayer)
        {
            data = GetComponent<CardUI>().card.data;
            if (data is MinionData)
                currHealth = baseHP = (data as MinionData).baseHealth;
            else
                currHealth = baseHP = (data as Ally).baseHp;
        }
        else
        {
            var identity = GetComponent<Player>().identity.alterEgo;
            currHealth = baseHP = identity.baseHP;
        }
    }

    public void TakeDamage(int damage)
    {
        _damage = damage;
        Task _subject = Task.Factory.StartNew(() => takingDamage.Notify());
        Task.WaitAll(_subject);

        if (_damage > 0)
        {
            if (tough)
            {
               tough = false;
               return;
            }

            currHealth -= _damage;

            if (currHealth <= 0)
            {
                //GetComponent<IDestructable>().WhenDefeated(); func in event trigger
            }
        }

        gameObject.SendMessage("Refresh");
    }

    public void RecoverHealth(int healing)
    {
        currHealth += healing;

        if (currHealth > baseHP)
            currHealth = baseHP;

        GetComponent<HealthUI>().Refresh();
    }

    public void IncreaseMaxHealth(int amount)
    {
        baseHP += 3;
        RecoverHealth(3);
        GetComponent<HealthUI>().Refresh();
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
