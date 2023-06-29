using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

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

    public event UnityAction AttackInitiated;
    public event UnityAction ThwartInitiated;
    public event UnityAction SchemeInitiated;

    public CharacterStats(Player owner, HeroData hData, AlterEgoData aData)
    {
        Owner = owner;
        Attacker = new Attacker(owner, hData);
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

    public CharacterStats(ICard owner, CardData data)
    {
        Owner = owner;
        Attacker = new Attacker(owner, data);
        Health = new Health(owner, data);

        if (owner is MinionCard)
            Schemer = new Schemer(owner as MinionCard, data as MinionCardData);
        else
            Thwarter = new Thwarter(owner as AllyCard, data as AllyCardData);
    }

    public async Task InitiateAttack(AttackAction _attack = null)
    {
        AttackAction attack = Attacker.Attack(_attack);

        if (attack == null)
            return;

        AttackInitiated?.Invoke();
        await AttackSystem.instance.InitiateAttack(attack);

        if (Owner is AllyCard)
            Health.TakeDamage((Owner as AllyCard).AttackConsq);
    }
    public async Task InitiateScheme()
    {
        SchemeAction scheme = Schemer.Scheme();

        SchemeInitiated?.Invoke();
        if (scheme == null)
            return;

        await SchemeSystem.instance.InitiateScheme(scheme);
    }
    public async Task InitiateThwart(ThwartAction _thwart = null)
    {
        ThwartAction thwart = Thwarter.Thwart(_thwart);

        ThwartInitiated?.Invoke();
        if (thwart == null)
            return;

        await ThwartSystem.instance.InitiateThwart(thwart);

        if (Owner is AllyCard)
            Health.TakeDamage((Owner as AllyCard).ThwartConsq);
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
