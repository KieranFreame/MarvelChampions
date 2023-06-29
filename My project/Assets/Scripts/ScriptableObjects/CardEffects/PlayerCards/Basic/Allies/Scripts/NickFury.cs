using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Nick Fury", menuName = "MarvelChampions/Card Effects/Basic/Nick Fury")]
public class NickFury : PlayerCardEffect
{
    private List<string> effectDesc = new();
    public override async Task OnEnterPlay(Player owner, PlayerCard card)
    {
        _owner = owner;
        Card = card;

        effectDesc.Clear();
        effectDesc.Add("Remove 2 Threat from a Scheme");
        effectDesc.Add("Draw 3 cards from your Deck");
        effectDesc.Add("Deal 4 damage to an enemy");

        int value = await ChooseEffectUI.ChooseEffect(effectDesc);
        TurnManager.OnEndVillainPhase += DiscardSelf;

        switch (value)
        {
            case 1: //Thwart 2
                await ThwartSystem.instance.InitiateThwart(new(2));
                break;
            case 2: //Draw 3
                DrawCardSystem.instance.DrawCards(new(3));
                break;
            case 3: //Damage 4
                List<ICharacter> enemies = new() { FindObjectOfType<Villain>() };
                enemies.AddRange(FindObjectsOfType<MinionCard>());
                await DamageSystem.instance.ApplyDamage(new(enemies, 4, false));
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
