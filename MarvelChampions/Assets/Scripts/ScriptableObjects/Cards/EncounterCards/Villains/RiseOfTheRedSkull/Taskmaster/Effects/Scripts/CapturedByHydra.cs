using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Captured By Hydra", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Captured By Hydra")]
public class CapturedByHydra : EncounterCardEffect
{
    AllyCardData captive;

    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        captive = HuntingDownHeroes.GetCaptive();

        ScenarioManager.inst.MainScheme.Threat.Acceleration++;

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration--;

        TurnManager.instance.CurrPlayer.Deck.limbo.Add(captive);
        TurnManager.instance.CurrPlayer.Hand.AddToHand(CreateCardFactory.Instance.CreateCard(captive, GameObject.Find("PlayerHandTransform").transform) as PlayerCard);

        ScenarioManager.inst.RemoveFromGame(Card.Data);
        Destroy(Card);

        return Task.CompletedTask;
    }
}
