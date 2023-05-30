using System.Collections.Specialized;
using UnityEngine;


[CreateAssetMenu(fileName = "CrisisInterdiction", menuName = "MarvelChampions/Card Effects/Captain Marvel/Crisis Interdiction")]
public class CrisisInterdiction : PlayerCardEffect
{
    Threat prevTarget;

    public override void OnEnterPlay(Player owner, Card card)
    {
        _owner = owner;

        _owner.StartCoroutine(ThwartSystem.instance.InitiateThwart(new(2)));

        if (_owner.Identity.IdentityTraits.Contains("Aerial".ToLower()))
        {
            ThwartSystem.OnThwartComplete += SecondThwart;
        }
    }

    private void SecondThwart()
    {
        TargetSystem.instance.candidates.CollectionChanged += CandidateAdded;
        prevTarget = ThwartSystem.instance.Target;

        _owner.StartCoroutine(ThwartSystem.instance.InitiateThwart(new(2)));

        ThwartSystem.OnThwartComplete -= SecondThwart;
        ThwartSystem.OnThwartComplete += CleanUp;
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

    private void CleanUp()
    {
        TargetSystem.instance.candidates.CollectionChanged -= CandidateAdded;
        ThwartSystem.OnThwartComplete -= CleanUp;
    }
}
