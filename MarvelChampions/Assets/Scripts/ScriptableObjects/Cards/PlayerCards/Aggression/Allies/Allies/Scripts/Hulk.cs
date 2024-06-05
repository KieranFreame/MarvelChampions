using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Hulk", menuName = "MarvelChampions/Card Effects/Aggression/Hulk")]
public class Hulk : PlayerCardEffect
{
    readonly List<ICharacter> enemies = new();

    public override Task OnEnterPlay()
    {
        AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);
        (Card as AllyCard).CanThwart = false;
        return Task.CompletedTask;
    }

    public void IsTriggerMet(AttackAction action)
    {
        if (action.Owner == Card as ICharacter)
            EffectResolutionManager.Instance.ResolvingEffects.Push(this);
    }

    public override async Task Resolve()
    {
        _owner.Deck.Mill(1);
        var card = _owner.Deck.discardPile.Last() as PlayerCardData;

        Debug.Log("Discarded " + card.cardName + ". Resource is " + card.cardResources[0].ToString());

        switch (card.cardResources[0])
        {
            case Resource.Physical:
                await PhysicalEffect();
                break;
            case Resource.Energy:
                await EnergyEffect();
                break;
            case Resource.Scientific:
                ScientificEffect();
                break;
            case Resource.Wild:
                await PhysicalEffect();
                await EnergyEffect();
                ScientificEffect();
                break;
            default:
                Debug.LogError("Top card doesn't have a resource");
                break;
        }

        enemies.Clear();
    }

    private async Task PhysicalEffect()
    {
        enemies.Add(ScenarioManager.inst.ActiveVillain);
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);
        await DamageSystem.Instance.ApplyDamage(new(enemies, 2, card:Card));
    }

    private async Task EnergyEffect()
    {
        enemies.Add(ScenarioManager.inst.ActiveVillain);
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);
        enemies.Add(_owner);
        enemies.AddRange(_owner.CardsInPlay.Allies);
        await DamageSystem.Instance.ApplyDamage(new(enemies, 1, true, card:Card));
    }

    private void ScientificEffect()
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
        _owner.CardsInPlay.Allies.Remove(Card as AllyCard);
        _owner.Deck.Discard(Card);
    }

    public override void OnExitPlay()
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
    }
}
