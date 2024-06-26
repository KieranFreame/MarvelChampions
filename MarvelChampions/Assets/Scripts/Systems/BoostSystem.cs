using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

public class BoostSystem
{
    private static BoostSystem instance;

    public static BoostSystem Instance
    {
        get
        {
            instance ??= new BoostSystem();
            return instance;
        }
    }

    BoostSystem()
    {
        PauseMenu.OnRestartGame += Restart;
    }

    private void Restart()
    {
        PauseMenu.OnRestartGame -= Restart;
        instance = null;
    }

    public int BoostCardCount { get; set; } = 1;
    private readonly List<CardData> _boostCards = new();
    public Action Action { get; }
    public delegate Task<int> ModifyBoost(int boostIcons);
    public List<ModifyBoost> Modifiers { get; private set; } = new();

    public void DealBoostCards()
    {
        for (int i = 0; i < BoostCardCount; i++)
            _boostCards.Add(ScenarioManager.inst.EncounterDeck.DealCard());

        BoostCardCount = 1;
    }

    public async Task<int> FlipCard(Action action)
    {
        int value = 0;

        while (_boostCards.Count > 0)
        {
            EncounterCard inst = CreateCardFactory.Instance.CreateCard(_boostCards[0], GameObject.Find("EncounterCards").transform) as EncounterCard;

            int boostIcons = inst.BoostIcons;

            for (int i = Modifiers.Count - 1; i >= 0; i--)
            {
                boostIcons = await Modifiers[i](boostIcons);
            }

            Debug.Log($"{inst.CardName} is boosting the {action.Owner}'s Activation by +{boostIcons}");

            value += boostIcons;

            if (inst.Boost != null)
                await EffectManager.Inst.AddEffect(inst, inst.Boost);

            await Task.Delay(2000);
            _boostCards.RemoveAt(0);

            if (!inst.InPlay)
                ScenarioManager.inst.EncounterDeck.Discard(inst);
        }

        //OnBoostCardsResolved?.Invoke(value);
        _boostCards.Clear();
        return value;
    }
}