using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterCardUI : CardUI
{
    private EncounterCard encounterCard;

    [Header("Encounter Card UI")]
    [SerializeField] private Image cardArt;
    [SerializeField] private Image boostIcon;
    [SerializeField] private Transform boostIconParent;
    [SerializeField] private TMP_Text effectText;
    [SerializeField] private TMP_Text nameText;

    protected virtual void OnEnable()
    {
        if (encounterCard == null)
        {
            TryGetComponent(out encounterCard);

            if (encounterCard == null)
                transform.parent.TryGetComponent(out encounterCard);

            encounterCard.SetupComplete += LoadData;
        }
    }

    protected override void LoadData()
    {
        if (nameText != null) nameText.text = encounterCard.CardName;
        if (effectText != null) effectText.text = encounterCard.CardDesc;

        if (boostIconParent != null && boostIconParent.childCount > 0)
            for (int i = 0; i < boostIconParent.childCount; i++)
                Destroy(boostIconParent.GetChild(i).gameObject);

        for (int i = 0; i < encounterCard.BoostIcons; i++)
        {
            Instantiate(boostIcon, boostIconParent);
        }

        if (cardArt != null)
            cardArt.sprite = CardArt;

        encounterCard.SetupComplete -= LoadData;
    }
}
