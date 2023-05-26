using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jessica Jones", menuName = "MarvelChampions/Card Effects/Justice/Jessica Jones")]
public class JessicaJones : PlayerCardEffect
{
    Thwarter thwarter;

    public override void OnEnterPlay(Player owner, Card card)
    {
        _owner = owner;
        _card = card;

        thwarter = (_card as AllyCard).CharStats.Thwarter;

        thwarter.CurrentThwart += ScenarioManager.sideSchemes.Count;

        ScenarioManager.sideSchemes.CollectionChanged += ChangeTHW;
    }

    private void ChangeTHW(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                thwarter.CurrentThwart++;
                break;
            case NotifyCollectionChangedAction.Remove:
                thwarter.CurrentThwart--;
                break;
        }
    }

    public override void OnExitPlay()
    {
        ScenarioManager.sideSchemes.CollectionChanged -= ChangeTHW;
    }
}
