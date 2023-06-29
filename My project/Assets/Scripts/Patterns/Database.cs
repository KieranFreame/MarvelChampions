using System.Linq;
using UnityEngine;

public class Database : MonoBehaviour
{
    public CardDatabase cards;
    public CardSetDatabase cardSets;

    private static Database inst;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    public static CardData GetCardDataById(string id)
    {
        return inst.cards.database.FirstOrDefault(i => i.cardID == id);
    }

    public static CardSet GetCardSetByName(string name)
    {
        return inst.cardSets.database.FirstOrDefault(i => i.name == name);
    }
}
