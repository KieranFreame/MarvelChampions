using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Jessica Jones", menuName = "MarvelChampions/Card Effects/Justice/Jessica Jones")]
public class JessicaJones : PlayerCardEffect
{
    Thwarter thwarter;

    public override async Task OnEnterPlay(Player owner, PlayerCard card)
    {
        _owner = owner;
        Card = card;

        thwarter = (Card as AllyCard).CharStats.Thwarter;

        thwarter.CurrentThwart += ScenarioManager.sideSchemes.Count;

        ScenarioManager.sideSchemes.CollectionChanged += ChangeTHW;
        await Task.Yield();
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
