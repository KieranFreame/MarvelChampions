using System.Linq;
using UnityEngine;

public class Database : MonoBehaviour
{
    public CardDatabase cards;
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
}
