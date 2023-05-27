using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

/// <summary>
/// Temporary Home for new StateMachine-less Villain Turn Controller.
/// </summary>
public class StatelessVTC : MonoBehaviour
{
    readonly List<MinionCard> minionsInPlay = new();
    MainSchemeCard mainScheme;
    Villain ActiveVillain { get; set; }
    public int HazardCount { get; set; }

    private void OnEnable()
    {
        mainScheme = FindObjectOfType<MainSchemeCard>();

        TurnManager.OnEndPlayerPhase += StartVillainPhase;
    }

    private void StartVillainPhase() => StartCoroutine(VillainPhase());

    private IEnumerator VillainPhase()
    {
        yield return StartCoroutine(mainScheme.Accelerate());

        foreach (Player p in TurnManager.Players)
        {
            if (p.Identity.ActiveIdentity is Hero)
                yield return StartCoroutine(ActiveVillain.CharStats.InitiateAttack());
            else
                yield return StartCoroutine(ActiveVillain.CharStats.InitiateScheme());
        }

        if (minionsInPlay.Count > 0)
        {
            foreach (Player p in TurnManager.Players)
            {
                yield return StartCoroutine(ActivateMinions(p));
            }
        }
            
        
        DealEncounterCards();

        yield return StartCoroutine(RevealEncounterCards());

        //ChangeFirstPlayer
        TurnManager.instance.EndVillainPhase();
    }

    private IEnumerator ActivateMinions(Player p)
    {
        foreach (MinionCard m in minionsInPlay)
        {
            if (p.Identity.ActiveIdentity is Hero)
                yield return StartCoroutine(m.CharStats.InitiateAttack());
            else
                yield return StartCoroutine(m.CharStats.InitiateScheme());
        }

        yield return null;
    }

    private void DealEncounterCards()
    {
        int cardsToDeal = TurnManager.Players.Count + HazardCount;

        while (cardsToDeal > 0)
        {
            foreach (Player p in TurnManager.Players) 
            {
                p.EncounterCards.AddCard(ActiveVillain.EncounterDeck.deck[0]);
                ActiveVillain.EncounterDeck.Deal();
                cardsToDeal--;

                if (cardsToDeal == 0)
                    break;
            }
        }
    }

    private IEnumerator RevealEncounterCards()
    {
        foreach (Player p in TurnManager.Players)
        {
            yield return StartCoroutine(p.EncounterCards.RevealEncounterCards());
        }
    }
}
