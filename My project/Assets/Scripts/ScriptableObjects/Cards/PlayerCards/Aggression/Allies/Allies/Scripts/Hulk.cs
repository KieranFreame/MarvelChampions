using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Hulk", menuName = "MarvelChampions/Card Effects/Aggression/Hulk")]
public class Hulk : PlayerCardEffect
{
    readonly List<ICharacter> enemies = new();

    public override async Task OnEnterPlay()
    {
        (Card as AllyCard).CharStats.AttackInitiated += AttackInitiated;
        await Task.Yield();
    }

    private void AttackInitiated() => AttackSystem.OnAttackComplete += AttackComplete;

    private async void AttackComplete(Action action)
    {
        AttackSystem.OnAttackComplete -= AttackComplete;

        var attack = action as AttackAction;

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
        enemies.Add(FindObjectOfType<Villain>());
        enemies.AddRange(FindObjectsOfType<MinionCard>());
        await DamageSystem.instance.ApplyDamage(new(enemies, 2));
    }

    private async Task EnergyEffect()
    {
        enemies.Add(FindObjectOfType<Villain>());
        enemies.AddRange(FindObjectsOfType<MinionCard>());
        enemies.Add(_owner);
        enemies.AddRange(_owner.CardsInPlay.Allies);
        await DamageSystem.instance.ApplyDamage(new(enemies, 1, true));
    }

    private void ScientificEffect()
    {
        AttackSystem.OnAttackComplete -= AttackComplete;
        _owner.CardsInPlay.Allies.Remove(Card as AllyCard);
        _owner.Deck.Discard(Card);
    }
}
