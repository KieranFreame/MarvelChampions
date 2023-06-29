using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "CrisisInterdiction", menuName = "MarvelChampions/Card Effects/Captain Marvel/Crisis Interdiction")]
public class CrisisInterdiction : PlayerCardEffect
{
    Threat prevTarget;

    public override bool CanBePlayed()
    {
        return FindObjectsOfType<Threat>().Any(x => x.CurrentThreat > 0);
    }

    public override async Task OnEnterPlay(Player owner, PlayerCard card)
    {
        _owner = owner;

        await ThwartSystem.instance.InitiateThwart(new(2));

        if (_owner.Identity.IdentityTraits.Contains("Aerial"))
        {
            prevTarget = ThwartSystem.instance.Target;
            List<SchemeCard> schemes = new();
            schemes.AddRange(FindObjectsOfType<SchemeCard>());

            //If there are no schemes, or the previous scheme is the only target.
            if (schemes.Count() == 0 || (schemes.Count() == 1 && schemes[0] == prevTarget))
                return;

            ThwartSystem.OnThwartComplete += SecondThwart;
        }
    }

    private async void SecondThwart()
    {
        TargetSystem.instance.candidates.CollectionChanged += CandidateAdded;
        
        await ThwartSystem.instance.InitiateThwart(new(2));

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
