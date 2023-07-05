using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EncounterCard : MonoBehaviour, ICard
{
    public Villain Owner { get; private set; }
    public int BoostIcons { get; set; }
    public EncounterCardData Data { get; set; }
    public EncounterCardEffect Effect { get; set; }
    public Zone CurrZone { get; set; }
    public Zone PrevZone { get; set; }
    public bool InPlay { get; set; }
    public bool FaceUp { get; set; }
    public string CardName { get => Data.cardName; }
    public string CardDesc { get => Data.cardDesc; }

    public event UnityAction OnBoost;
    public event UnityAction SetupComplete;

    protected virtual void WhenDefeated()
    {
        Effect.OnExitPlay();
        ScenarioManager.inst.EncounterDeck.Discard(this);
    }
    public async Task OnRevealCard()
    {
        await Effect.OnEnterPlay(Owner, this, FindObjectOfType<Player>());
    }
    public void OnBoostCard() => OnBoost?.Invoke();
    public virtual void LoadCardData(EncounterCardData data, Villain owner)
    {
        //EncounterCard
        Owner = owner;
        BoostIcons = data.boostIcons;

        Data = data;
        GetComponent<CardUI>().CardArt = Data.cardArt;

        if (Data.effect != null)
            Effect = Instantiate(Data.effect);

        SetupComplete?.Invoke();
    }

    public void Flip() { return; }
}
