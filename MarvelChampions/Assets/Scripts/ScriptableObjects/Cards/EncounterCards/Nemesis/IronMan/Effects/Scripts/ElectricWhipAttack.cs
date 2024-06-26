using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Electric Whip Attack", menuName = "MarvelChampions/Card Effects/Nemesis/Iron Man/Electric Whip Attack")]
public class ElectricWhipAttack : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;

        if (!player.CardsInPlay.Permanents.Any(x => x.CardType == CardType.Upgrade))
        {
            ScenarioManager.inst.Surge(player);
            return;
        }

        if (player.Identity.ActiveIdentity is not Hero)
        {
            await SecondEffect(player);
        }
        else
        {
            int choice = await ChooseEffectUI.ChooseEffect(new List<string>()
            {
               "Deal 1 damage to yourself for each upgrade you control",
               "Choose and discard an upgrade you control"
            });

            if (choice == 1)
            {
                int damage = player.CardsInPlay.Permanents.Count(x => x.Data.cardType == CardType.Upgrade);
                DamageAction action = new(player, damage, card: Card);
                await DamageSystem.Instance.ApplyDamage(action);
            }
            else
            {
                await SecondEffect(player);
            }
        }
    }

    private async Task SecondEffect(Player _player)
    {
        List<PlayerCard> playerCards = new();

        playerCards.AddRange(_player.CardsInPlay.Permanents);
        playerCards.RemoveAll(x => x.Data.cardType != CardType.Upgrade);

        Debug.Log("Discard an Upgrade you control");
        PlayerCard p = await TargetSystem.instance.SelectTarget(playerCards);

        _player.CardsInPlay.Permanents.Remove(p);
        _player.Deck.Discard(p);
    }

    public override async Task Boost(Action action)
    {
        if (action is not AttackAction) return;
        if (DefendSystem.Instance.Target != null) return;

        var attack = action as AttackAction;
        await SecondEffect(attack.Target as Player);
    }
}
