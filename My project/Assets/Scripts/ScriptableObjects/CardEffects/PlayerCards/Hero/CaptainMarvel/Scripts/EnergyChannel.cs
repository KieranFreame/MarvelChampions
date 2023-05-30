using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy Channel", menuName = "MarvelChampions/Card Effects/Captain Marvel/Energy Channel")]
public class EnergyChannel : PlayerCardEffect
{
    private int counters = 0;
    [SerializeField, TextArea] private string effectA;
    [SerializeField, TextArea] private string effectB;

    public override void OnEnterPlay(Player owner, Card card)
    {
        _owner = owner;
        _card = card;

        if (counters != 0) { counters = 0; }
    }

    public override bool CanActivate()
    {
        return true;
    }

    public override void Activate()
    {
        if (counters == 0) //can't deal damage so default to adding counters
        {
            _card.StartCoroutine(AddCounters());
            return;
        }

        UIManager.ChooseEffect(new List<string> { effectA, effectB });
        ChooseEffectUI.EffectSelected += ChosenEffect;
    }

    private void ChosenEffect(int effectIndex)
    {
        ChooseEffectUI.EffectSelected -= ChosenEffect;

        switch (effectIndex)
        {
            case 1:
                _card.StartCoroutine(AddCounters());
                break;
            case 2:
                DealDamage();
                break;
        }
    }

    private IEnumerator AddCounters()
    {
        Debug.Log("Adding Counters to Energy Channel");

        yield return _card.StartCoroutine(PayCostSystem.instance.GetResources(Resource.Energy));
        counters += PayCostSystem.instance.Resources.Count;

        //TODO: Add functionality to display counters (Another script)
        Debug.Log("Added " + PayCostSystem.instance.Resources.Count.ToString() + " counters to Energy Channel");
    }

    private void DealDamage()
    {
        var action = new AttackAction(attack: (counters * 2 <= 10 ? counters * 2 : 10), new List<TargetType>() { TargetType.TargetMinion, TargetType.TargetVillain }, owner : _owner);
        _owner.StartCoroutine(AttackSystem.instance.InitiateAttack(action));

        _owner.Deck.Discard(_card);
    }
}
