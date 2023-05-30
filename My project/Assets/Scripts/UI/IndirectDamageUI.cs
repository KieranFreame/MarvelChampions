using System.Collections;
using System.Collections.Generic;
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
        damageText ??= transform.Find("DamageText").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        damageText.text = totalDamage.ToString();
    }

    public IEnumerator SetIndirectDamage(ICharacter h, System.Action<int> callback)
    {
        target = h;

        while (!finished)
            yield return null;

        callback(totalDamage);

        totalDamage = 0;
        finished = false;
    }

    public void IncreaseDamage()
    {
        totalDamage++;
        Mathf.Clamp(totalDamage, 0, target.CharStats.Health.CurrentHealth);
    }

    public void DecreaseDamage()
    {
        totalDamage--;
        Mathf.Clamp(totalDamage, 0, target.CharStats.Health.CurrentHealth);
    }
}
