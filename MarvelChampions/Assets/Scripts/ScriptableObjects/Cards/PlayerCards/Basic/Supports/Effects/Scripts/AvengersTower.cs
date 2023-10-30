using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Avengers Tower", menuName = "MarvelChampions/Card Effects/Basic/Supports/Avengers Tower")]
public class AvengersTower : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        if (_owner.CardsInPlay.Allies.Count() > 0 && _owner.CardsInPlay.Allies.Where(x => x.CardTraits.Contains("Avenger") == false).Count() == 0)
        {
            _owner.CardsInPlay.AllyLimit++;
        }

        _owner.CardsInPlay.Allies.CollectionChanged += AlliesChanged;

        foreach (AllyCard a in _owner.CardsInPlay.Allies)
        {
            a.CardTraits.CollectionChanged += TraitChanged;
        }

        return Task.CompletedTask;
    }

    #region Passive
    private async void AlliesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        var collection = sender as ObservableCollection<AllyCard>;

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (AllyCard a in e.NewItems)
                {
                    if (a.CardTraits.Contains("Avenger"))
                    {
                        if (collection.Count() == 1)
                            _owner.CardsInPlay.AllyLimit++;
                    }
                    else //non-Avenger
                    {
                        if (_owner.CardsInPlay.Allies.Count == 1) return;

                        if (_owner.CardsInPlay.Allies.Where(x => !x.CardTraits.Contains("Avenger")).Count() == 1) //if all other allies are avengers
                        {
                            _owner.CardsInPlay.AllyLimit--;

                            if (_owner.CardsInPlay.ReachedAllyLimit())
                            {
                                await PlayCardSystem.Instance.DiscardAlly();
                            };
                        }
                    }

                    a.CardTraits.CollectionChanged += TraitChanged;
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (AllyCard a in e.OldItems)
                {
                    if (a.CardTraits.Contains("Avenger"))
                    {
                        if (collection.Count == 0)
                            _owner.CardsInPlay.AllyLimit--;
                    }
                    else //non-Avenger
                    {
                        if (_owner.CardsInPlay.Allies.Count == 0) return;

                        if (_owner.CardsInPlay.Allies.Where(x => !x.CardTraits.Contains("Avenger")).Count() == 0) //if all other allies are avengers
                        {
                            _owner.CardsInPlay.AllyLimit++;
                        }
                    }

                    a.CardTraits.CollectionChanged -= TraitChanged;
                }
                break;
        }
    }
    private async void TraitChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems.Contains("Avenger"))
                {
                    if (_owner.CardsInPlay.Allies.Count == 1) //only ally
                        _owner.CardsInPlay.AllyLimit++;
                    else if (_owner.CardsInPlay.Allies.Any(x => !x.CardTraits.Contains("Avenger")) == false) //only avengers
                        _owner.CardsInPlay.AllyLimit++;
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems.Contains("Avenger"))
                {
                    if (_owner.CardsInPlay.Allies.Count == 1) //only ally
                        _owner.CardsInPlay.AllyLimit--;
                    else
                    {
                        if (_owner.CardsInPlay.Allies.Where(x => !x.CardTraits.Contains("Avenger")).Count() == 1) //all other allies are avengers
                        {
                            _owner.CardsInPlay.AllyLimit--;

                            if (_owner.CardsInPlay.ReachedAllyLimit())
                            {
                                await PlayCardSystem.Instance.DiscardAlly();
                            };
                        }
                    }
                }
                break;
        }
    }
    #endregion

    #region Action
    public override bool CanActivate()
    {
        if (Card.Exhausted)
            return false;

        return true;
    }

    public override Task Activate()
    {
        foreach (var item in _owner.Hand.cards.Where(x => x is AllyCard))
        {
            if (item.CardTraits.Contains("Avenger"))
            {
                item.CardCost--;
            }
        }

        PlayCardSystem.Instance.OnCardPlayed += OnCardPlayed;
        TurnManager.OnEndPlayerPhase += OnEndPhase;
        return Task.CompletedTask;
    }

    private void OnEndPhase()
    {
        foreach (var item in _owner.Hand.cards.Where(x => x is AllyCard))
        {
            if (item.CardTraits.Contains("Avenger"))
            {
                item.CardCost++;
            }
        }

        PlayCardSystem.Instance.OnCardPlayed -= OnCardPlayed;
        TurnManager.OnEndPlayerPhase -= OnEndPhase;
    }

    private void OnCardPlayed(PlayerCard card)
    {
        PlayCardSystem.Instance.OnCardPlayed -= OnCardPlayed;
        TurnManager.OnEndPlayerPhase -= OnEndPhase;

        if (card is AllyCard && card.CardTraits.Contains("Avenger"))
        {
            card.CardCost++;

            foreach (var item in _owner.Hand.cards.Where(x => x is AllyCard))
            {
                if (item.CardTraits.Contains("Avenger"))
                {
                    item.CardCost++;
                }
            }
        }
    }


    #endregion

    public override async void OnExitPlay()
    {
        if (_owner.CardsInPlay.Allies.Any(x => !x.CardTraits.Contains("Avenger"))) //if there are only avengers
        {
            _owner.CardsInPlay.AllyLimit--;

            if (_owner.CardsInPlay.ReachedAllyLimit())
            {
                await PlayCardSystem.Instance.DiscardAlly();
            };
        }
    }
}
