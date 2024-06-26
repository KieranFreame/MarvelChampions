using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Knight", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Goblin Knight")]
public class GoblinKnight : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        GameStateManager.Instance.OnActivationCompleted += CanRespond;
        return Task.CompletedTask;
    }

    private async void CanRespond(Action action)
    {
        if (action is not AttackAction || ((AttackAction)action).Card == Card) return;

        await EffectManager.Inst.AddEffect(_card, this);
    }

    public override async Task Resolve()
    {
        EncounterCardData data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
        ScenarioManager.inst.EncounterDeck.Mill();

        if (data is MinionCardData && data.cardTraits.Contains("Goblin"))
        {
            MinionCard goblin = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
            VillainTurnController.instance.MinionsInPlay.Add(goblin);
            await goblin.Effect.OnEnterPlay();
        }
    }
    public override Task WhenDefeated()
    {
        GameStateManager.Instance.OnActivationCompleted -= CanRespond;
        return Task.CompletedTask;
    }
}
