using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public Attacker Attacker { get; private set; }
    public Schemer Schemer { get; private set; }
    public Thwarter Thwarter { get; private set; }
    public Health Health { get; private set; }
    public Defender Defender { get; private set; }
    public Recovery Recovery { get; private set; }

    public CharacterStats(Identity owner, HeroData hData, AlterEgoData aData)
    {
        Attacker = new Attacker(owner, hData);
        Thwarter = new Thwarter(owner, hData);
        Defender = new Defender(owner, hData);
        Health = new Health(owner, aData);
        Recovery = new Recovery(owner, aData);
    }

    public CharacterStats(Villain owner)
    {
        Attacker = new Attacker(owner);
        Schemer = new Schemer(owner);
        Health = new Health(owner);
    }

    public CharacterStats(Card owner, CardData data)
    {
        Attacker = new Attacker(owner, data);
        Health = new Health(owner, data);

        if (owner is MinionCard)
            Schemer = new Schemer(owner as MinionCard, data as MinionCardData);
        else
            Thwarter = new Thwarter(owner as AllyCard, data as AllyCardData);
    }

    public IEnumerator InitiateAttack()
    {
        AttackAction attack = Attacker.Attack();

        if (attack == null)
            yield break;

        /*yield return StartCoroutine()*/
        AttackSystem.instance.InitiateAttack(attack);

        if (Attacker.Owner is AllyCard)
            Health.TakeDamage(1);
    }

    public IEnumerator InitiateScheme()
    {
        SchemeAction scheme = Schemer.Scheme();

        if (scheme == null)
            yield break;

        /*yield return StartCoroutine()*/
        SchemeSystem.instance.InitiateScheme(scheme);
    }

    public IEnumerator InitiateThwart()
    {
        ThwartAction thwart = Thwarter.Thwart();

        if (thwart == null)
            yield break;

        ThwartSystem.InitiateThwart(thwart);

        if (Thwarter.Owner is AllyCard)
            Health.TakeDamage(1);
    }

    public void InitiateRecover()
    {
        Recovery.Recover();
    }
}
