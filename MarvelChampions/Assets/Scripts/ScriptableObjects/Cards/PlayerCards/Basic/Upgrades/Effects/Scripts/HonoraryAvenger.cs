using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Honorary Avenger", menuName = "MarvelChampions/Card Effects/Basic/Upgrades/Honorary Avenger")]
public class HonoraryAvenger : PlayerCardEffect
{
    AllyCard ally;
    bool addedTrait = false;

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.CardsInPlay.Allies.Count == 0)
                return false;

            if (!_owner.Identity.IdentityTraits.Contains("Avenger"))
                return false;

            return true;
        }

        return false;
    }
    public override async Task OnEnterPlay()
    {
        ally = await TargetSystem.instance.SelectTarget(_owner.CardsInPlay.Allies.ToList());

        ally.Attachments.Add(_card as IAttachment);

        _card.transform.SetParent(ally.transform, false);
        _card.transform.SetAsFirstSibling();
        _card.transform.localPosition = new Vector3(-30, 0, 0);

        if (ally.CardTraits.Contains("Avenger") == false)
        {
            ally.CardTraits.Add("Avenger");
            addedTrait = true;
        }

        ally.CharStats.Health.IncreaseMaxHealth(1);
    }

    public override void OnExitPlay()
    {
        if (ally == null) return;

        ally.Attachments.Remove(_card as IAttachment);

        if (addedTrait)
            ally.CardTraits.Remove("Avenger");

        ally.CharStats.Health.IncreaseMaxHealth(1);
    }
}
