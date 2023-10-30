using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Flame-Soldier", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Hydra Assault/Hydra Flame-Soldier")]
public class HydraFlameSoldier : EncounterCardEffect
{
    bool undefended = false;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;

        return Task.CompletedTask;
    }

    private void AttackInitiated()
    {
        DefendSystem.Instance.OnDefenderSelected += DefenderSelected;
    }

    private void DefenderSelected(ICharacter arg0)
    {
        undefended = (arg0 == null);
        AttackSystem.Instance.OnAttackCompleted.Add(AttackComplete);
    }

    private async Task AttackComplete(AttackAction action)
    {
        if (undefended)
        {
            await DiscardSupport(action);
        }

        return;
    }

    async Task DiscardSupport(AttackAction action)
    {
        Player p = (action.Target as Player);
        List<PlayerCard> playerCards = new();

        playerCards.AddRange(p.CardsInPlay.Permanents.Where(x => x.CardType is CardType.Support));

        if (playerCards.Count == 0)
        {
            return;
        }
        else if (playerCards.Count == 1)
        {
            p.CardsInPlay.Permanents.Remove(playerCards[0]);
            p.Deck.Discard(playerCards[0]);
        }
        else
        {
            Debug.Log("Discard a Support you control");
            PlayerCard c = await TargetSystem.instance.SelectTarget(playerCards);

            p.CardsInPlay.Permanents.Remove(c);
            p.Deck.Discard(c);
        }
    }

    public override async Task Boost(Action action)
    {
        if (action is AttackAction && !DefendSystem.Instance.Defended)
        {
            await DiscardSupport(action as AttackAction);
        }
    }
}
