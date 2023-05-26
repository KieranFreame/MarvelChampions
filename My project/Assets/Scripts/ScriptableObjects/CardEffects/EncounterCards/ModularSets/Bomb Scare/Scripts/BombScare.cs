using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb Scare", menuName = "MarvelChampions/Card Effects/Bomb Scare/Bomb Scare")]
public class BombScare : CardEffect
{
    /// <summary>
    /// When Revealed: Place an additional 1 threat here per player
    /// Acceleration Icon
    /// </summary>

    public override void OnEnterPlay(Villain owner, Card card)
    {
        card.GetComponent<Threat>().GainThreat(1 * TurnManager.Players.Count);

        FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().Acceleration += 1;
    }

    public override void OnExitPlay()
    {
        FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().Acceleration -= 1;
    }
}
