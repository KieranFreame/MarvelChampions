using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Night Nurse", menuName = "MarvelChampions/Card Effects/Protection/Supports/The Night Nurse")]
public class NightNurse : PlayerCardEffect
{
    Counters medical;

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            //Unique
            if (_owner.CardsInPlay.Permanents.Any(x => x.CardName == _card.CardName))
                return false;

            return true;
        }

        return false;
    }

    public override Task OnEnterPlay()
    {
        medical = _card.gameObject.AddComponent<Counters>();
        medical.AddCounters(3);

        return Task.CompletedTask;
    }

    public override bool CanActivate()
    {
        if (_card.Exhausted)
            return false;

        if (!_owner.CharStats.Afflicted() && !_owner.CharStats.Health.Damaged())
            return false;

        return true;
    }

    public override async Task Activate()
    {
        _card.Exhaust();
        List<Status> targetStatus = PopulateStatusList(_owner);
        Status toRemove = Status.None;
        if (targetStatus.Count > 1)
        {
            List<string> statuses = targetStatus.Cast<string>().ToList();
            int decision = await ChooseEffectUI.ChooseEffect(statuses);
            toRemove = targetStatus[decision - 1];
        }
        else if (targetStatus.Count == 1)
        {
            toRemove = targetStatus[0];
        }
            

        switch (toRemove)
        {
            case Status.Stunned:
                _owner.CharStats.Attacker.Stunned = false;
                break;
            case Status.Confused:
                _owner.CharStats.Confusable.Confused = false;
                break;
            case Status.Tough:
                _owner.CharStats.Health.Tough = false;
                break;
            default:
                break;
        }

        _owner.CharStats.Health.CurrentHealth += 1;

        medical.RemoveCounters(1);

        if (medical.CountersLeft == 0)
        {
            _owner.Deck.Discard(_card);
        }
    }

    private List<Status> PopulateStatusList(ICharacter target)
    {
        List<Status> targetStatus = new List<Status>();

        if (target.CharStats.Attacker.Stunned)
            targetStatus.Add(Status.Stunned);
        if (target.CharStats.Confusable.Confused)
            targetStatus.Add(Status.Confused);
        if (target.CharStats.Health.Tough)
            targetStatus.Add(Status.Tough);

        return targetStatus;
    }
}
