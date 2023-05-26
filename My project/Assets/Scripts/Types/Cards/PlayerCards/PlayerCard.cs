using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCard : Card, IExhaust
{
    private bool _exhausted = false;
    private Animator _animator;

    private int cardCost;

    public bool Exhausted { get => _exhausted; set => _exhausted = value; }

    public event UnityAction OnActivate;
    public event UnityAction<int> CardCostChanged;

    protected virtual void OnEnable()
    {
        TurnManager.OnEndPlayerPhase += Ready;
    }
    protected virtual void OnDisable()
    {
        TurnManager.OnEndPlayerPhase -= Ready;
    }

    public override void LoadCardData(CardData data, GameObject owner)
    {
        CurrZone = Zone.Hand;
        cardCost = (data as PlayerCardData).cardCost;
        base.LoadCardData(data, owner);
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
            if (TryGetComponent<Animator>(out _animator))
                _animator.Play("Exhaust");
            _exhausted = true;
        }
    }
    public List<Resource> Resources { get => (Data as PlayerCardData).cardResources; }
    public int CardCost 
    { 
        get => cardCost; 
        set
        {
            cardCost = value;
            CardCostChanged?.Invoke(cardCost);
        }
    }
    public Aspect CardAspect { get => (Data as PlayerCardData).cardAspect; }
}
