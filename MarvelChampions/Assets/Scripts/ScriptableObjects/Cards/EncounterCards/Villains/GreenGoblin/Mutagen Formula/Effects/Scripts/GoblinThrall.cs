using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Thrall", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Goblin Thrall")]
public class GoblinThrall : EncounterCardEffect
{
    Guard _guard;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _guard = new(card as MinionCard);
        return Task.CompletedTask;
    }

    public override async Task Boost(Action action)
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Goblin Thrall").GetComponent<EncounterCard>();

        card.InPlay = true;
        card.transform.SetParent(RevealEncounterCardSystem.Instance.MinionTransform);
        VillainTurnController.instance.MinionsInPlay.Add(card as MinionCard);
        await card.Effect.OnEnterPlay(ScenarioManager.inst.ActiveVillain, card, TurnManager.instance.CurrPlayer);
    }
}
