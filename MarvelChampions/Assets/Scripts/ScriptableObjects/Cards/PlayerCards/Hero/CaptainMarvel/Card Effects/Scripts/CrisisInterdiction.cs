using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "CrisisInterdiction", menuName = "MarvelChampions/Card Effects/Captain Marvel/Crisis Interdiction")]
public class CrisisInterdiction : PlayerCardEffect
{
    SchemeCard prevTarget;

    public override bool CanBePlayed()
    {
        return base.CanBePlayed() && ScenarioManager.inst.ThreatPresent();
    }

    public override async Task OnEnterPlay()
    {
        if (_owner.Identity.IdentityTraits.Contains("Aerial"))
            ThwartSystem.Instance.OnThwartComplete.Add(IsTriggerMet);

        await _owner.CharStats.InitiateThwart(new(2, Owner));
    }

    private void IsTriggerMet(ThwartAction action)
    {   
        ThwartSystem.Instance.OnThwartComplete.Remove(IsTriggerMet);

        List<SchemeCard> schemes = new() { ScenarioManager.inst.MainScheme };
        schemes.AddRange(ScenarioManager.sideSchemes);

        //If there are no schemes, or the previous scheme is the only target.
        if (!ScenarioManager.inst.ThreatPresent() || (schemes.Count() == 1 && schemes[0] == action.Target))
            return;

        prevTarget = action.Target;
        EffectManager.Inst.Resolving.Push(this);
    }

    public override async Task Resolve()
    {
        TargetSystem.instance.candidates.CollectionChanged += CandidateAdded;

        await _owner.CharStats.InitiateThwart(new(2, Owner));

        TargetSystem.instance.candidates.CollectionChanged -= CandidateAdded;
    }

    private void CandidateAdded(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems.Contains(prevTarget))
                   ((ObservableCollection<dynamic>)sender).Remove(prevTarget);
                break;
        }
    }
}
