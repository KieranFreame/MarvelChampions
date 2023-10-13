using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStats
{
    //For accessing coroutines
    private readonly ICharacter Owner;
    public Attacker Attacker { get; private set; }
    public Schemer Schemer { get; private set; }
    public Thwarter Thwarter { get; private set; }
    public Health Health { get; set; }
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
        Health = new Health(owner, aData);
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
        Owner = owner as ICharacter;
        Attacker = new Attacker(owner, data);
        Health = new Health(owner, data);

        if (owner is MinionCard)
            Schemer = new Schemer(owner as MinionCard, data as MinionCardData);
        else
            Thwarter = new Thwarter(owner as AllyCard, data as AllyCardData);
    }

    public async Task<bool> InitiateAttack(AttackAction _attack = null)
    {
        _attack = await Attacker.Attack(_attack);

        if (_attack == null) return false;

        AttackInitiated?.Invoke();

        await AttackSystem.Instance.InitiateAttack(_attack);

        if (Owner is AllyCard)
        {
            Health.TakeDamage(new(Owner, (Owner as AllyCard).AttackConsq));
        }

        return true;
            
    }
    public async Task<bool> InitiateScheme()
    {
        SchemeAction scheme = await Schemer.Scheme();

        if (scheme == null) return false;

        SchemeInitiated?.Invoke();

        await SchemeSystem.Instance.InitiateScheme(scheme);

        return true;
    }
    public async Task<bool> InitiateThwart(ThwartAction _thwart = null)
    {
        ThwartAction thwart = Thwarter.Thwart(_thwart);

        if (thwart == null) return false;

        ThwartInitiated?.Invoke();

        await ThwartSystem.Instance.InitiateThwart(thwart);

        if (Owner is AllyCard)
            Health.TakeDamage(new(Owner, (Owner as AllyCard).ThwartConsq));

        return true;
    }
    public void InitiateRecover()
    {
        Recovery.Recover();
    }

    public bool Afflicted()
    {
        return (Attacker.Stunned || Confusable.Confused || Health.Tough);
    }

    public IConfusable Confusable
    {
        get
        {
            return Thwarter as IConfusable ?? Schemer;
        }
    }
}
