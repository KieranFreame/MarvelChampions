using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Red Dagger", menuName = "MarvelChampions/Card Effects/Ms Marvel/Red Dagger")]
public class RedDagger : PlayerCardEffect
{
    public override async Task WhenDefeated()
    {
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PayCostSystem.instance.GetResources(amount: 2);

            List<ICharacter> enemies = new List<ICharacter>() { ScenarioManager.inst.ActiveVillain };
            enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

            await DamageSystem.Instance.ApplyDamage(new(enemies, 2));

            (Card as AllyCard).CharStats.Health.RecoverHealth(3);

            foreach (IAttachment card in (Card as AllyCard).Attachments)
                _owner.Deck.Discard(card as ICard);

            Card.PrevZone = Card.CurrZone;
            Card.CurrZone = Zone.Hand;

            Card.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
            _owner.Hand.AddToHand(Card);
            Card.InPlay = false;
        }
    }
}
