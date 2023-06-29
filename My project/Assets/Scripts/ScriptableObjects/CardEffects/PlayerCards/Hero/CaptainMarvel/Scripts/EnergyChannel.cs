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

    public override async Task OnEnterPlay(Player owner, PlayerCard card)
    {
        _owner = owner;
        Card = card;

        counters = Card.gameObject.AddComponent<Counters>();
        await Task.Yield();
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

        if (counters.CountersLeft >= 5)
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
        await PayCostSystem.instance.GetResources(Resource.Energy, 5);
        counters.AddCounters(PayCostSystem.instance.Resources.Count);
    }

    private async Task DealDamage()
    {
        var action = new AttackAction(attack: (counters.CountersLeft <= 10 ? counters.CountersLeft * 2 : 10), new List<TargetType>() { TargetType.TargetMinion, TargetType.TargetVillain }, owner : _owner);
        await AttackSystem.instance.InitiateAttack(action);

        _owner.CardsInPlay.Permanents.Remove(Card);
        _owner.Deck.Discard(Card);
    }
}
