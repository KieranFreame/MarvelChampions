using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventTrigger : MonoBehaviour
{
    private AbilityLoader abilityLoader;

    private void Start()
    {
        if (GetComponent<CardUI>().card.data != null)
            abilityLoader = GetComponent<CardUI>().card.abilityLoader;
    }

    void OnEnterPlay()
    {
        if (abilityLoader != null)
            abilityLoader.TriggerAbility();

        GetComponent<CardUI>().card.inPlay = true;
    }

    void AfterThwart()
    {
        if (abilityLoader != null)
            abilityLoader.TriggerAbility();
    }

    public void ActivateAbility()
    {
        if (abilityLoader != null)
            abilityLoader.TriggerAbility();
    }
}
