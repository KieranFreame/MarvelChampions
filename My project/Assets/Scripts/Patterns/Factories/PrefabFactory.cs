using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    public static PrefabFactory instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    [Header("Player Card Prefabs")]
    [SerializeField] private GameObject allyPrefab;
    [SerializeField] private GameObject permanentPrefab;
    [SerializeField] private GameObject eventPrefab;
    [SerializeField] private GameObject resourcePrefab;

    [Header("Encounter Card Prefabs")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private GameObject sideSchemePrefab;
    [SerializeField] private GameObject mainSchemePrefab;
    [SerializeField] private GameObject treacheryPrefab;
    [SerializeField] private GameObject attachmentPrefab;

    public GameObject CreatePlayerCard(PlayerCardData card)
    {
        switch (card.cardType)
        {
            case CardType.Ally: return allyPrefab;
            case CardType.Upgrade: 
            case CardType.Support:
                return permanentPrefab;
            case CardType.Event: return eventPrefab;
            case CardType.Resource: return resourcePrefab;
            default: return null;
        };
    }
    public GameObject CreateEncounterCard(EncounterCardData card)
    {
        return card.cardType switch
        {
            CardType.Minion => minionPrefab,
            CardType.Scheme => sideSchemePrefab,
            CardType.Treachery => treacheryPrefab,
            CardType.Attachment => attachmentPrefab,
            //EncounterCardType.MainScheme => mainSchemePrefab,
            _ => null,
        };
    }
}
