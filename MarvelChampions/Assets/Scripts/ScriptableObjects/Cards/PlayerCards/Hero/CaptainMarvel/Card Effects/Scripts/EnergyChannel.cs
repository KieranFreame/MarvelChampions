using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy Channel", menuName = "MarvelChampions/Card Effects/Captain Marvel/Energy Channel")]
public class EnergyChannel : PlayerCardEffect
{
    private Counters counters;
    [SerializeField, TextArea] private string effectA;
    [SerializeField, TextArea] private string effectB;

    public override Task OnEnterPlay()
    {
        counters = _card.gameObject.AddComponent<Counters>();
        return Task.CompletedTask;
    }

    public override bool CanActivate()
    {
        return true;
    }

    public override async Task Activate()
    {
        if (counters.CountersLeft == 0) //can't deal damage so default to adding counters
        {
            await AddCounters();
            return;
        }

        if (counters.CountersLeft >= 5) //can't deal more than 10, which 2x5 so do damage.
        {
            await DealDamage();
            return;
        }

        int effectIndex = await ChooseEffectUI.ChooseEffect(new List<string> { effectA, effectB });

        switch (effectIndex)
        {
            case 1:
                await AddCounters();
                break;
            case 2:
                await DealDamage();
                break;
        }
    }

    private async Task AddCounters()
    {
        await PayCostSystem.instance.GetResources(Resource.Energy, 5, true);
        counters.AddCounters(PayCostSystem.instance.Resources.Count);
    }

    private async Task DealDamage()
    {
        var action = new AttackAction(attack: (counters.CountersLeft <= 10 ? counters.CountersLeft * 2 : 10), targets: new() { TargetType.TargetVillain, TargetType.TargetMinion }, attackType: AttackType.Card, owner : _owner);
        await AttackSystem.Instance.InitiateAttack(action);

        _owner.CardsInPlay.Permanents.Remove(_card);
        _owner.Deck.Discard(_card);
    }
}
