using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Red Dagger", menuName = "MarvelChampions/Card Effects/Ms Marvel/Red Dagger")]
public class RedDagger : PlayerCardEffect
{
    public override async Task WhenDefeated()
    {
        bool decision = await ConfirmActivateUI.MakeChoice(_card);

        if (decision)
        {
            await PayCostSystem.instance.GetResources(new() { { Resource.Any, 2 } });

            List<ICharacter> enemies = new List<ICharacter>() { ScenarioManager.inst.ActiveVillain };
            enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

            await DamageSystem.Instance.ApplyDamage(new(enemies, 2));

            (_card as AllyCard).CharStats.Health.CurrentHealth += 3;

            foreach (IAttachment card in (_card as AllyCard).Attachments)
                _owner.Deck.Discard(card as ICard);

            _card.PrevZone = Card.CurrZone;
            _card.CurrZone = Zone.Hand;

            _card.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
            _owner.Hand.AddToHand(_card);
            _card.InPlay = false;
        }
    }
}
