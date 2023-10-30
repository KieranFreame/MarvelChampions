using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "CrisisInterdiction", menuName = "MarvelChampions/Card Effects/Captain Marvel/Crisis Interdiction")]
public class CrisisInterdiction : PlayerCardEffect
{
    SchemeCard prevTarget;

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            return (ScenarioManager.sideSchemes.Any(x => x.Threat.CurrentThreat > 0) && ScenarioManager.inst.MainScheme.Threat.CurrentThreat > 0);
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        ThwartAction action = new(2);
        await ThwartSystem.Instance.InitiateThwart(action);

        if (_owner.Identity.IdentityTraits.Contains("Aerial"))
        {
            prevTarget = action.Target;
            List<SchemeCard> schemes = new();
            schemes.AddRange(FindObjectsOfType<SchemeCard>());

            //If there are no schemes, or the previous scheme is the only target.
            if (schemes.Count() == 0 || (schemes.Count() == 1 && schemes[0].Threat == prevTarget.Threat))
                return;

            ThwartSystem.OnThwartComplete += SecondThwart;
        }
    }

    private async void SecondThwart()
    {
        TargetSystem.instance.candidates.CollectionChanged += CandidateAdded;
        
        await ThwartSystem.Instance.InitiateThwart(new(2));

        ThwartSystem.OnThwartComplete -= SecondThwart;
        TargetSystem.instance.candidates.CollectionChanged -= CandidateAdded;
    }

    private void CandidateAdded(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems.Contains(prevTarget))
                    e.NewItems.Remove(prevTarget);
                break;
        }
    }
}
