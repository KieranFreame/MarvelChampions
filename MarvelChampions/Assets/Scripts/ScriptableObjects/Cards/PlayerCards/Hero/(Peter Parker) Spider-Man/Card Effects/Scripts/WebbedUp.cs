using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Webbed Up", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Webbed Up")]
public class WebbedUp : PlayerCardEffect
{
    readonly List<ICharacter> enemies = new();
    ICharacter target;

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
        target = await TargetSystem.instance.SelectTarget(enemies);

        target.Attachments.Add(Card as IAttachment);

        if (target is Villain)
        {
            Card.transform.SetParent(GameObject.Find("AttachmentTransform").transform);
        }
        else
        {
            Card.transform.SetParent((target as MonoBehaviour).transform, false);
            Card.transform.SetAsFirstSibling();
            Card.transform.localPosition = new Vector3(-30, 0, 0);
        }

        target.CharStats.Attacker.AttackCancel.Add(CancelAttack);
    }

    public Task<AttackAction> CancelAttack(AttackAction action)
    {
        target.CharStats.Attacker.AttackCancel.Remove(CancelAttack);
        target.CharStats.Attacker.Stunned = true;
        target.Attachments.Remove(Card as IAttachment);

        _owner.CardsInPlay.Permanents.Remove(Card);
        _owner.Deck.Discard(Card);

        action = null;
        return Task.FromResult(action);
    }
}
