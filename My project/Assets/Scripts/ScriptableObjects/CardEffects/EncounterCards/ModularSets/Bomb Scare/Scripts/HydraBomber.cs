using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Bomber", menuName = "MarvelChampions/Card Effects/Bomb Scare/Hydra Bomber")]
public class HydraBomber : EncounterCardEffect
{
    [SerializeField] private string takeDamage;
    [SerializeField] private string placeThreat;
    
    /// <summary>
    /// When Revealed: Choose to either take 2 damage, or place 1 threat on the main scheme
    /// </summary>

    public override void OnEnterPlay(Villain owner, Card card)
    {
        UIManager.ChooseEffect(new List<string> { takeDamage, placeThreat });
        ChooseEffectUI.EffectSelected += ChosenEffect;
    }

    private void ChosenEffect(int effectIndex)
    {
        ChooseEffectUI.EffectSelected -= ChosenEffect;

        switch (effectIndex)
        {
            case 1:
                var p = FindObjectOfType<Player>();
                var h = p.CharStats.Health;
                _card.StartCoroutine(DamageSystem.ApplyDamage(new DamageAction(h, 2)));
                break;
            case 2:
                FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().GainThreat(1);
                break;
        }
    }
}
