using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Nick Fury", menuName = "MarvelChampions/Card Effects/Basic/Allies/Nick Fury")]
public class NickFury : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        int value = await ChooseEffectUI.ChooseEffect(new List<string>()
        {
            "Remove 2 Threat from a Scheme",
            "Draw 3 cards from your Deck",
            "Deal 4 damage to an enemy"
        });

        TurnManager.OnEndVillainPhase += DiscardSelf;

        switch (value)
        {
            case 1: //Thwart 2
                await ThwartSystem.Instance.InitiateThwart(new(2));
                break;
            case 2: //Draw 3
                DrawCardSystem.Instance.DrawCards(new(3));
                break;
            case 3: //Damage 4
                List<ICharacter> enemies = new() { FindObjectOfType<Villain>() };
                enemies.AddRange(FindObjectsOfType<MinionCard>());
                await DamageSystem.Instance.ApplyDamage(new(enemies, 4, false, card: Card));
                break;
        }


        await Task.Yield();
    }

    private void DiscardSelf()
    {
        TurnManager.OnEndVillainPhase -= DiscardSelf;
        _owner.Deck.Discard(Card);
        _owner.CardsInPlay.Allies.Remove(Card as AllyCard);
    }

    public override void OnExitPlay()
    {
        TurnManager.OnEndVillainPhase -= DiscardSelf;
    }
}
