using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        TurnManager.OnStartVillainPhase += StartVillainPhase;
    }

    private void OnDisable()
    {
        TurnManager.OnStartVillainPhase -= StartVillainPhase;
    }

    public int HazardCount { get; set; }

    #region Fields
    bool villainActivated;
    public ObservableCollection<MinionCard> MinionsInPlay { get; private set; } = new();
    #endregion

    #region Delegates
    public delegate Task ActivationComplete();
    public List<ActivationComplete> OnActivationComplete { get; private set; } = new();
    #endregion

    private void StartVillainPhase()
    {
        VillainPhase();
    }

    private async void VillainPhase()
    {
        Debug.Log("Step 1: Accelerating");
        await ScenarioManager.inst.MainScheme.Accelerate();

        Debug.Log("Step 2: The Villain Activates");
        foreach (Player p in TurnManager.Players)
        {
            if (p.Identity.ActiveIdentity is Hero)
            {
                villainActivated = await ScenarioManager.inst.ActiveVillain.CharStats.InitiateAttack();
            }
            else
            {
                villainActivated = await ScenarioManager.inst.ActiveVillain.CharStats.InitiateScheme();
            }

            if (villainActivated)
            {
                foreach (var task in OnActivationComplete)
                {
                    await task.Invoke();
                }
            }
        }

        Debug.Log("Step 3: The Minions Activate");
        if (MinionsInPlay.Count > 0)
        {
            foreach (Player p in TurnManager.Players)
            {
               await ActivateMinions(p);
            }
        }

        Debug.Log("Step 4: Encounter Cards are dealt out");
        DealEncounterCards();

        Debug.Log("Step 5: Encounter Cards are revealed");
        await RevealEncounterCards();

        Debug.Log("Step 6: Change the first player");
        //ChangeFirstPlayer

        Debug.Log("Step 7: Return to Player Phase");
        TurnManager.instance.EndVillainPhase();
    }

    private async Task ActivateMinions(Player p)
    {
        for (int i = MinionsInPlay.Count-1; i >= 0; i--)
        {
            if (p.Identity.ActiveIdentity is Hero)
                await MinionsInPlay[i].CharStats.InitiateAttack();
            else
                await MinionsInPlay[i].CharStats.InitiateScheme();
        }
    }

    private void DealEncounterCards()
    {
        int cardsToDeal = TurnManager.Players.Count + HazardCount;

        while (cardsToDeal > 0)
        {
            foreach (Player p in TurnManager.Players)
            {
                p.EncounterCards.AddCard(ScenarioManager.inst.EncounterDeck.DealCard());
                cardsToDeal--;

                if (cardsToDeal == 0)
                    break;
            }
        }
    }

    private async Task RevealEncounterCards()
    {
        foreach (Player p in TurnManager.Players)
        {
            await p.EncounterCards.RevealEncounterCards();
        }
    }
}
