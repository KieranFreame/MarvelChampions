using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Thrall", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Goblin Thrall")]
public class GoblinThrall : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        AttackSystem.Instance.Guards.Add((MinionCard)_card);
        GameStateManager.Instance.OnCharacterDefeated += WhenDefeated;
        return Task.CompletedTask;
    }

    public void WhenDefeated(ICharacter defeated)
    {
        if (defeated is not MinionCard || defeated as MinionCard != _card as MinionCard)
            return;

        AttackSystem.Instance.Guards.Remove((MinionCard)_card);
        GameStateManager.Instance.OnCharacterDefeated -= WhenDefeated;
    }

    public override async Task Boost(Action action)
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Goblin Thrall").GetComponent<EncounterCard>();

        card.InPlay = true;
        card.transform.SetParent(RevealEncounterCardSystem.Instance.MinionTransform);
        VillainTurnController.instance.MinionsInPlay.Add(card as MinionCard);
        await card.Effect.OnEnterPlay();
    }
}
