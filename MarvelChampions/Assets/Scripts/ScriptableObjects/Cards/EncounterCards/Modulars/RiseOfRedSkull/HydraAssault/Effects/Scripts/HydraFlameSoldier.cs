using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Main Effect: After this makes an undefended attack, discard a support you control
/// Boost: If this is boosting an undefended attack, discard a support you control.
/// </summary>

[CreateAssetMenu(fileName = "Hydra Flame-Soldier", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Hydra Assault/Hydra Flame-Soldier")]
public class HydraFlameSoldier : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        DefendSystem.Instance.OnDefenderSelected += DefenderSelected;

        return Task.CompletedTask;
    }

    private void DefenderSelected(ICharacter target, AttackAction action)
    {
        if (action.Card == Card)
            if (target == null)
                Debug.Log("FUCK");//EffectManager.Inst.Resolving.Push(this);
    }

    public override async Task Resolve()
    {
        Player p = TurnManager.instance.CurrPlayer;
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
            await Resolve();
        }
    }
}
