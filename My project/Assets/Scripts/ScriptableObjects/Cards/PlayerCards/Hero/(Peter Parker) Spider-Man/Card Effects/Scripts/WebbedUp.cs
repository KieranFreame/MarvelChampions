using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Webbed Up", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Webbed Up")]
public class WebbedUp : PlayerCardEffect, ICancelAttack
{
    List<ICharacter> enemies = new();
    ICharacter target;

    public override bool CanBePlayed()
    {
        enemies.Clear();

        enemies.Add(FindObjectOfType<Villain>());
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        enemies.RemoveAll(x => x.Attachments.FirstOrDefault(x => (x as ICard).CardName == Card.CardName) != default);

        return enemies.Count > 0;
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

        target.CharStats.AttackCancel.Add(this);
    }

    public AttackAction CancelAttack()
    {
        target.CharStats.AttackCancel.Remove(this);
        target.CharStats.Attacker.Stunned = true;
        target.Attachments.Remove(Card as IAttachment);

        _owner.CardsInPlay.Permanents.Remove(Card);
        _owner.Deck.Discard(Card);

        return null;
    }
}
