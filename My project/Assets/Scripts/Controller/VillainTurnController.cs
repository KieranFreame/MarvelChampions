using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VillainTurnController : MonoBehaviour
{
    public static VillainTurnController instance;

    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        ActiveVillain ??= FindObjectOfType<Villain>();
        mainScheme ??= FindObjectOfType<MainSchemeCard>();

        TurnManager.OnStartVillainPhase += StartVillainPhase;
    }

    private void OnDisable()
    {
        TurnManager.OnStartVillainPhase -= StartVillainPhase;
    }

    public int HazardCount { get; set; }

    #region Fields
    public List<MinionCard> MinionsInPlay { get; private set; } = new();
    private Villain ActiveVillain;
    MainSchemeCard mainScheme;
    #endregion

    private void StartVillainPhase() => StartCoroutine(VillainPhase());

    private IEnumerator VillainPhase()
    {
        Debug.Log("Step 1: Accelerating");
        yield return StartCoroutine(mainScheme.Accelerate());

        Debug.Log("Step 2: The Villain Activates");
        foreach (Player p in TurnManager.Players)
        {
            if (p.Identity.ActiveIdentity is Hero)
                yield return StartCoroutine(ActiveVillain.CharStats.InitiateAttack());
            else
                yield return StartCoroutine(ActiveVillain.CharStats.InitiateScheme());
        }

        Debug.Log("Step 3: The Minions Activate");
        if (MinionsInPlay.Count > 0)
        {
            foreach (Player p in TurnManager.Players)
            {
                yield return StartCoroutine(ActivateMinions(p));
            }
        }

        Debug.Log("Step 4: Encounter Cards are dealt out");
        DealEncounterCards();

        Debug.Log("Step 5: Encounter Cards are revealed");
        yield return StartCoroutine(RevealEncounterCards());

        Debug.Log("Step 6: Change the first player");
        //ChangeFirstPlayer

        Debug.Log("Step 7: Return to Player Phase");
        TurnManager.instance.EndVillainPhase();
    }

    private IEnumerator ActivateMinions(Player p)
    {
        foreach (MinionCard m in MinionsInPlay)
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
