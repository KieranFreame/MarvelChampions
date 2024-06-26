using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield Toss", menuName = "MarvelChampions/Card Effects/Captain America/Shield Toss")]
public class ShieldToss : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (!_owner.CardsInPlay.Permanents.Any(x => x.CardName == "Captain America's Shield"))
                return false;

            //Shield Toss is the only card in hand
            if (_owner.Hand.cards.Count == 1)
                return false;

            return true;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        if (_owner.CharStats.Attacker.Stunned)
        {
            _owner.CharStats.Attacker.Stunned = false;
            return;
        }

        List<ICharacter> enemies = new();
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        if (AttackSystem.Instance.Guards.Count == 0)
            enemies.Add(ScenarioManager.inst.ActiveVillain);

        var token = FinishButton.ToggleFinishButton(true, TargetSystem.instance.FinishedSelecting);
        var discards = await TargetSystem.instance.SelectTargets(_owner.Hand.cards.ToList(), enemies.Count, token);
        var targets = await TargetSystem.instance.SelectTargets(enemies, discards.Count, default);

        foreach (ICharacter target in targets)
        {
            await AttackSystem.Instance.InitiateAttack(new(4, target, owner: _owner, attackType: AttackType.Card, card: Card));
            _owner.Hand.Discard(discards[0]);
            discards.RemoveAt(0);
        }

        PlayerCard shield = _owner.CardsInPlay.Permanents.First(x => x.CardName == "Captain America's Shield");

        shield.Ready();

        shield.PrevZone = shield.CurrZone;
        shield.CurrZone = Zone.Hand;

        shield.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
        _owner.Hand.AddToHand(shield);
        shield.InPlay = false;

        shield.Effect.OnExitPlay();
    }
}
