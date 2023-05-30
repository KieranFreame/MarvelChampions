using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    //For accessing coroutines
    private readonly dynamic Owner;
    public Attacker Attacker { get; private set; }
    public Schemer Schemer { get; private set; }
    public Thwarter Thwarter { get; private set; }
    public Health Health { get; private set; }
    public Defender Defender { get; private set; }
    public Recovery Recovery { get; private set; }

    public CharacterStats(Player owner, HeroData hData, AlterEgoData aData)
    {
        Owner = owner;
        Attacker = new Attacker(owner.Identity, hData);
        Thwarter = new Thwarter(owner.Identity, hData);
        Defender = new Defender(owner.Identity, hData);
        Health = new Health(owner.Identity, aData);
        Recovery = new Recovery(owner.Identity, Health, aData);
    }

    public CharacterStats(Villain owner)
    {
        Owner = owner;
        Attacker = new Attacker(owner);
        Schemer = new Schemer(owner);
        Health = new Health(owner);
    }

    public CharacterStats(Card owner, CardData data)
    {
        Owner = owner;
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

        yield return AttackSystem.instance.InitiateAttack(attack);

        if (Owner is AllyCard)
            Health.TakeDamage((Owner as AllyCard).attackConsq);
    }
    public IEnumerator InitiateScheme()
    {
        SchemeAction scheme = Schemer.Scheme();

        if (scheme == null)
            yield break;

        yield return Owner.StartCoroutine(SchemeSystem.instance.InitiateScheme(scheme));
    }
    public IEnumerator InitiateThwart()
    {
        ThwartAction thwart = Thwarter.Thwart();

        if (thwart == null)
            yield break;

        yield return ThwartSystem.instance.InitiateThwart(thwart);

        if (Owner is AllyCard)
            Health.TakeDamage((Owner as AllyCard).thwartConsq);
    }
    public void InitiateRecover()
    {
        Recovery.Recover();
    }

    public IConfusable Confusable
    {
        get
        {
            return Thwarter as IConfusable ?? Schemer;
        }
    }
}
