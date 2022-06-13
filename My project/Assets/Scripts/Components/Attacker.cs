using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public int _attack { get; set; }
    public List<string> keywords = new List<string>(); //temp?
    public int baseATK { get; set; }
    public bool stunned = false;
    [SerializeField]
    private bool isPlayer;
    private CardData data;

    private void Start()
    {
        if (!isPlayer)
        {
            if (GetComponent<CardUI>().card.data is Ally)
            {
                Ally ally = GetComponent<CardUI>().card.data as Ally;
                _attack = baseATK = ally.combatant.baseAttack;
            }
            else
            {
                MinionData minion = GetComponent<CardUI>().card.data as MinionData;
                _attack = baseATK = minion.combatant.baseAttack;
            }
            
        }
        else
        {
            HeroData hero = transform.parent.GetComponent<Player>().identity.hero;
            _attack = baseATK = hero.baseATK;
        }
    }

    public void Attack()
    {
        if (!isPlayer)
        {
            if (GetComponent<CardUI>().card.exhausted)
                return;

            GetComponent<CardUI>().card.exhausted = true;

            var animator = GetComponent<Animator>();
            if (animator != null)
                animator.Play("Exhaust");
        }
        else
        {
            Identity i = transform.parent.GetComponent<Player>().identity;
            if (i.exhausted)
                return;

            i.exhausted = true;

            var animator = transform.parent.Find("IdentityProfile").GetComponent<Animator>();
            if (animator != null)
                animator.Play("Exhaust");
        }

        if (stunned)
        {
            stunned = false;
            return;
        }

        var attack = new AttackAction(owner:this);
        attack.Execute();
    }

    public void Refresh()
    {
        if (!isPlayer)
        {
            if (GetComponent<CardUI>().card.data is Ally)
            {
                Ally ally = GetComponent<CardUI>().card.data as Ally;
                _attack = baseATK = ally.combatant.attack;
            }
            else
            {
                MinionData minion = GetComponent<CardUI>().card.data as MinionData;
                _attack = baseATK = minion.combatant.attack;
            }
        }
    }
}
