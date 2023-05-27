using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Captain Marvel", menuName = "MarvelChampions/Identity Effects/Captain Marvel/Hero")]
public class CaptainMarvel : IdentityEffect
{
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
        hasActivated = false;

        TurnManager.OnStartPlayerPhase += Reset;
    }

    public override bool CanActivate()
    {
        if (!owner.CharStats.Health.Damaged())
        {
            return false;
        }

        if (owner.Hand.cards.FirstOrDefault(x => x.Resources.Contains(Resource.Energy)) == null) //need to account for Permanents
        {
            return false;
        }

        return !hasActivated;
    }

    public override void Activate()
    {
        owner.StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {
        yield return owner.StartCoroutine(PayCostSystem.instance.GetResources(Resource.Energy, 1));

        owner.GetComponent<Health>().RecoverHealth(1);
        DrawCardSystem.instance.DrawCards(new(1));

        hasActivated = true;
    }
}
