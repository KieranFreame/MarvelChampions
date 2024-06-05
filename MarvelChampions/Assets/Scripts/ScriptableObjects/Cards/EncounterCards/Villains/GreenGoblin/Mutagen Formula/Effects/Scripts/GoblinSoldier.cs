using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Soldier", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Goblin Soldier")]
public class GoblinSoldier : EncounterCardEffect
{
    Player p;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;
        p = player;

        return Task.CompletedTask;
    }

    public override async Task WhenDefeated()
    {
        await DamageSystem.Instance.ApplyDamage(new(p, 1, card: Card, owner: _owner));
    }

    public override async Task Boost(Action action)
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Goblin Soldier").GetComponent<EncounterCard>();

        card.InPlay = true;
        card.transform.SetParent(RevealEncounterCardSystem.Instance.MinionTransform);
        VillainTurnController.instance.MinionsInPlay.Add(card as MinionCard);
        await card.Effect.OnEnterPlay(ScenarioManager.inst.ActiveVillain, card, TurnManager.instance.CurrPlayer);
    }
}
