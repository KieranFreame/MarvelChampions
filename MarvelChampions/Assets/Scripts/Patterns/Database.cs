using System.Linq;
using UnityEngine;

public class Database : MonoBehaviour
{
    public IdentityDatabase identities;
    public CardDatabase cards;
    private static Database inst;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static CardData GetCardDataById(string id)
    {
        return inst.cards.database.FirstOrDefault(i => i.cardID == id);
    }

    //Search using Alter-Ego due to Spider-Man (Peter Parker) & Spider-Man (Miles Morales) existing
    public static IdentityContainer GetIdentityContainerByAlterEgo(string alterEgoName)
    {
        return inst.identities.database.FirstOrDefault(i => i.alterEgoData.alterEgoName == alterEgoName);
    }

    public static Database Instance => inst;
}
