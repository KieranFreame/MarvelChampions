using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCard : MonoBehaviour, ICard, IExhaust
{
    public PlayerCardData Data;
    public Player Owner { get; private set; }

    private bool _exhausted = false;
    public Animator _animator;

    private int cardCost;
    public int BaseCardCost { get; protected set; }

    public bool Exhausted { get => _exhausted; set => _exhausted = value; }

    public event UnityAction SetupComplete;
    public event UnityAction<int> CardCostChanged;

    protected virtual void OnEnable()
    {
        TurnManager.OnEndPlayerPhase += Ready;
    }

    protected virtual void OnDisable()
    {
        if ((InPlay || Owner.Hand.Contains(this)) && Effect != null)
            Effect.OnExitPlay();

        TurnManager.OnEndPlayerPhase -= Ready;
    }

    private void OnDestroy()
    {
        if ((InPlay || Owner.Hand.Contains(this)) && Effect != null)
            Effect.OnExitPlay();

        TurnManager.OnEndPlayerPhase -= Ready;
    }

    public virtual void LoadCardData(PlayerCardData data, Player owner)
    {
        Owner = owner;
        Data = data;

        cardCost = BaseCardCost = Data.cardCost;
        
        GetComponent<CardUI>().CardArt = Data.cardArt;

        foreach (string trait in Data.cardTraits)
            CardTraits.Add(trait);

        if (Data.effect != null)
        {
            Effect = Instantiate(Data.effect);
            Effect.LoadEffect(Owner, this);
        }

        SetupComplete?.Invoke();
    }

    public async Task OnEnterPlay()
    {
        await Effect.OnEnterPlay();
    }
    public virtual void Ready()
    {
        if (_exhausted)
        {
            if (TryGetComponent<Animator>(out _animator))
                _animator.Play("Ready");

            _exhausted = false;
        }
    }
    public virtual void Exhaust()
    {
        if (!_exhausted)
        {
            if (TryGetComponent(out _animator))
                _animator.Play("Exhaust");
            _exhausted = true;
        }
    }

    public PlayerCardEffect Effect { get; private set; }
    public virtual List<Resource> Resources { get => Data.cardResources; }
    public int CardCost 
    { 
        get => cardCost; 
        set
        {
            cardCost = value;

            if (cardCost < 0)
                cardCost = 0;

            CardCostChanged?.Invoke(cardCost);
        }
    }
    public Aspect CardAspect { get => Data.cardAspect; }
    public Zone CurrZone { get; set; }
    public Zone PrevZone { get; set; }
    public bool InPlay { get; set; }
    public bool FaceUp { get; set; }
    public string CardName { get => Data.cardName; }
    public string CardDesc { get => Data.cardDesc; }
    public CardType CardType { get => Data.cardType; }
    public ObservableCollection<string> CardTraits { get; protected set; } = new();
}
