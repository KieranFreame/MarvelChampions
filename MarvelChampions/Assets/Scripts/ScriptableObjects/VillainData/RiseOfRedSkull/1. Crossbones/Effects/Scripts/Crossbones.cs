using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Crossbones", menuName = "MarvelChampions/Villain Effects/RotRS/Crossbones")]
public class Crossbones : VillainEffect
{
    public override void LoadEffect(Villain owner)
    {
        base.LoadEffect(owner);
        _owner.Attachments.CollectionChanged += CheckForWeapons;
    }

    private void CheckForWeapons(object sender, NotifyCollectionChangedEventArgs e)
    {
        var attachments = sender as ObservableCollection<IAttachment>;

        if (attachments.Any(x => (x as ICard).CardTraits.Contains("Weapon")))
        {
            if (!_owner.CharStats.Attacker.Keywords.Contains("Piercing"))
                _owner.CharStats.Attacker.Keywords.Add("Piercing");
        }
        else
        {
            if (_owner.CharStats.Attacker.Keywords.Contains("Piercing"))
                _owner.CharStats.Attacker.Keywords.Remove("Piercing");
        }
    }

    public override Task StageOneEffect()
    {
        return base.StageOneEffect();
    }

    public override async Task StageTwoEffect()
    {
        EncounterCardData cbMachineGun = ScenarioManager.inst.EncounterDeck.Search("Crossbones' Machine Gun", true) as EncounterCardData;

        if (cbMachineGun == null)
            return;

        var card = CreateCardFactory.Instance.CreateCard(cbMachineGun, RevealEncounterCardSystem.Instance.AttachmentTransform) as AttachmentCard;

        _owner.Attachments.Add(card.Effect as IAttachment);
        await card.OnRevealCard();
    }

    public override Task StageThreeEffect()
    {
        WeaponsDeck.Instance.RevealTopWeapon();
        return Task.CompletedTask;
    }
}
