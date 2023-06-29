using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabFactory
{
    public static PrefabFactory instance = new();

    public GameObject CreatePlayerCard(PlayerCardData card)
    {
        return card.cardType switch
        {
            CardType.Ally => Resources.Load<GameObject>("Prefabs/AllyTemplate"),
            CardType.Upgrade => Resources.Load<GameObject>("Prefabs/UpgradeTemplate"),
            CardType.Support => Resources.Load<GameObject>("Prefabs/SupportTemplate"),
            CardType.Event => Resources.Load<GameObject>("Prefabs/EventTemplate"),
            CardType.Resource => Resources.Load<GameObject>("Prefabs/ResourceTemplate"),
            _ => null,
        };
        ;
    }
    public GameObject CreateEncounterCard(EncounterCardData card)
    {
        return card.cardType switch
        {
            CardType.Minion => Resources.Load<GameObject>("Prefabs/MinionTemplate"),
            CardType.SideScheme => Resources.Load<GameObject>("Prefabs/SideSchemeTemplate"),
            CardType.Treachery => Resources.Load<GameObject>("Prefabs/TreacheryTemplate"),
            CardType.Attachment => Resources.Load<GameObject>("Prefabs/AttachmentTemplate"),
            CardType.Obligation => Resources.Load<GameObject>("Prefabs/ObligationTemplate"),
            CardType.MainScheme => Resources.Load<GameObject>("Prefabs/MainSchemeTemplate"),
            _ => null,
        };
    }

    public static PrefabFactory Instance
    {
        get => instance;
    }
}
