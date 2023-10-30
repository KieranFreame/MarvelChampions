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
        (Card as AllyCard).CharStats.AttackInitiated += AttackInitiated;
        (Card as AllyCard).CanThwart = false;
        return Task.CompletedTask;
    }

    private void AttackInitiated() {AttackSystem.Instance.OnAttackCompleted.Add(AttackComplete); } 

    private async Task AttackComplete(Action action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackComplete);

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
        await DamageSystem.Instance.ApplyDamage(new(enemies, 2, card:Card));
    }

    private async Task EnergyEffect()
    {
        enemies.Add(FindObjectOfType<Villain>());
        enemies.AddRange(FindObjectsOfType<MinionCard>());
        enemies.Add(_owner);
        enemies.AddRange(_owner.CardsInPlay.Allies);
        await DamageSystem.Instance.ApplyDamage(new(enemies, 1, true, card:Card));
    }

    private void ScientificEffect()
    {
        (Card as AllyCard).CharStats.AttackInitiated -= AttackInitiated;
        _owner.CardsInPlay.Allies.Remove(Card as AllyCard);
        _owner.Deck.Discard(Card);
    }

    public override void OnExitPlay()
    {
        (Card as AllyCard).CharStats.AttackInitiated -= AttackInitiated;
    }
}
