using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nick Fury", menuName = "MarvelChampions/Card Effects/Basic/Nick Fury")]
public class NickFury : PlayerCardEffect
{
    [SerializeField] private string Thwart2;
    [SerializeField] private string Draw3;
    [SerializeField] private string Damage4;

    public override void OnEnterPlay(Player owner, Card card)
    {
        _owner = owner;
        _card = card;

        UIManager.ChooseEffect(new List<string>() { Thwart2, Draw3, Damage4 });
        ChooseEffectUI.EffectSelected += EffectChosen;

        TurnManager.OnEndVillainPhase += DiscardSelf;
    }

    private void EffectChosen(int value)
    {
        ChooseEffectUI.EffectSelected -= EffectChosen;

        switch (value)
        {
            case 1: //Thwart 2
                ThwartSystem.InitiateThwart(new(2));
                break;
            case 2: //Draw 3
                DrawCardSystem.instance.DrawCards(new(3));
                break;
            case 3: //Damage 4
                List<Health> enemies = new() { FindObjectOfType<Villain>().CharStats.Health };
                //enemies.AddRange(FindObjectsOfType<MinionCard>());
                
                _card.StartCoroutine(DamageSystem.ApplyDamage(new(enemies, 4, false)));
                break;
        }
    }

    private void DiscardSelf()
    {
        TurnManager.OnEndVillainPhase -= DiscardSelf;
        _owner.Deck.Discard(_card);
        _owner.GetComponent<Player>().CardsInPlay.Allies.Remove(_card as AllyCard);
    }

    public override void OnExitPlay()
    {
        TurnManager.OnEndVillainPhase -= DiscardSelf;
    }
}
