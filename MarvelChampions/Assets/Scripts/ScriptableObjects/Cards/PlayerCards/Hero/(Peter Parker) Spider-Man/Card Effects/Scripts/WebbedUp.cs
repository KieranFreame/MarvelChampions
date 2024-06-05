using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Webbed Up", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Webbed Up")]
public class WebbedUp : PlayerCardEffect, IAttachment
{
    readonly List<ICharacter> enemies = new();
    public ICharacter Attached { get; set; }

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            enemies.Clear();

            enemies.Add(FindObjectOfType<Villain>());
            enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

            enemies.RemoveAll(x => x.Attachments.FirstOrDefault(x => (x as ICard).CardName == Card.CardName) != default);

            return enemies.Count > 0;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        Attached = await TargetSystem.instance.SelectTarget(enemies);

        Attached.Attachments.Add(this);

        if (Attached is Villain)
        {
            _card.transform.SetParent(RevealEncounterCardSystem.Instance.AttachmentTransform);
        }
        else
        {
            _card.transform.SetParent((Attached as MonoBehaviour).transform, false);
            _card.transform.SetAsFirstSibling();
            _card.transform.localPosition = new Vector3(-30, 0, 0);
        }

        Attached.CharStats.Attacker.AttackCancel.Add(CancelAttack);
    }

    public Task<AttackAction> CancelAttack(AttackAction action)
    {
        Attached.CharStats.Attacker.AttackCancel.Remove(CancelAttack);
        Attached.CharStats.Attacker.Stunned = true;
        Attached.Attachments.Remove(this);

        _owner.CardsInPlay.Permanents.Remove(_card);
        _owner.Deck.Discard(Card);

        action = null;
        return Task.FromResult(action);
    }
}
