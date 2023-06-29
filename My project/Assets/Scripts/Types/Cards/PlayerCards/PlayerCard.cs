using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCard : MonoBehaviour, ICard, IExhaust
{
    public PlayerCardData Data;
    public Player Owner { get; private set; }

    private bool _exhausted = false;
    private Animator _animator;

    private int cardCost;
    public int BaseCardCost { get; protected set; }

    public bool Exhausted { get => _exhausted; set => _exhausted = value; }

    public event UnityAction OnActivate;
    public event UnityAction SetupComplete;
    public event UnityAction<int> CardCostChanged;

    protected virtual void OnEnable()
    {
        TurnManager.OnEndPlayerPhase += Ready;
    }
    protected virtual void OnDisable()
    {
        if (InPlay || Owner.Hand.Contains(this))
            Effect?.OnExitPlay();

        TurnManager.OnEndPlayerPhase -= Ready;
    }

    public virtual void LoadCardData(PlayerCardData data, Player owner)
    {
        Owner = owner;
        Data = data;

        CurrZone = Zone.Hand;
        cardCost = BaseCardCost = Data.cardCost;
        
        GetComponent<CardUI>().CardArt = Data.cardArt;

        Effect = Data.effect;

        SetupComplete?.Invoke();
    }

    public void Activate()
    {
        OnActivate?.Invoke();
    }
    public void Ready()
    {
        if (_exhausted)
        {
            if (TryGetComponent<Animator>(out _animator))
                _animator.Play("Ready");

            _exhausted = false;
        }
    }
    public void Exhaust()
    {
        if (!_exhausted)
        {
            if (TryGetComponent(out _animator))
                _animator.Play("Exhaust");
            _exhausted = true;
        }
    }

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
    public PlayerCardEffect Effect { get; set; }
    public Aspect CardAspect { get => Data.cardAspect; }
    public Zone CurrZone { get; set; }
    public Zone PrevZone { get; set; }
    public bool InPlay { get; set; }
    public bool FaceUp { get; set; }
    public string CardName { get => Data.cardName; }
    public string CardDesc { get => Data.cardDesc; }
}
