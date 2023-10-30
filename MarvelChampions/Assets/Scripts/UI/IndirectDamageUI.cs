using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class IndirectDamageUI : MonoBehaviour
{
    private TMP_Text damageText;
    private int totalDamage = 0;
    private bool finished = false;

    private ICharacter target;

    private void OnEnable()
    {
        damageText ??= transform.Find("DamageText").GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        damageText.text = totalDamage.ToString();
    }

    public async Task<int> SetIndirectDamage(ICharacter h)
    {
        totalDamage = 0;
        finished = false;
        target = h;

        while (!finished)
            await Task.Yield();

        return totalDamage;
    }

    public void IncreaseDamage()
    {
        totalDamage++;

        if (totalDamage > target.CharStats.Health.CurrentHealth)
            totalDamage = target.CharStats.Health.CurrentHealth;
        
    }

    public void DecreaseDamage()
    {
        totalDamage--;
        if (totalDamage < 0)
            totalDamage = 0;
    }

    public void ApplyDamage()
    {
        finished = true;
        gameObject.SetActive(false);
    }
}
