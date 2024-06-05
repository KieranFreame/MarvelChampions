using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterCardActions : MonoBehaviour
{
    private EncounterCard card;
    private GameObject activate;

    private void Awake()
    {
        activate = transform.Find("ActivateAbility").gameObject; 
        card = GetComponentInParent<EncounterCard>();
    }

    private void OnEnable()
    {
        activate.SetActive(card.Effect.CanActivate(TurnManager.instance.CurrPlayer));
    }

    public async void Activate()
    {
        gameObject.SetActive(false);
        await card.Effect.Activate(TurnManager.instance.CurrPlayer);
    }
}
