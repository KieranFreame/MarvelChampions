using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InvocationDeck
{
    private static InvocationDeck instance;
    public static InvocationDeck Instance
    {
        get
        {
            instance ??= new InvocationDeck();
            return instance;
        }
    }

    public List<PlayerCardData> Invocations;
    List<PlayerCardData> DiscardPile = new();

    public Transform invocationTransform;

    InvocationDeck()
    {
        invocationTransform = GameObject.Find("InvocationDeck").transform;

        Invocations = new List<PlayerCardData>()
        {
            Database.GetCardDataById("STRANGE-I-001") as PlayerCardData,
            Database.GetCardDataById("STRANGE-I-002") as PlayerCardData,
            Database.GetCardDataById("STRANGE-I-003") as PlayerCardData,
            Database.GetCardDataById("STRANGE-I-004") as PlayerCardData,
            Database.GetCardDataById("STRANGE-I-005") as PlayerCardData,
        };

        Shuffle();
        CreateCardFactory.Instance.CreateCard(Invocations[0], invocationTransform);
    }

    public async Task Activate()
    {
        await (Invocations[0].effect as IInvocation).Special();
    }

    public void Discard()
    {
        DiscardPile.Add(Invocations[0]);
        Invocations.RemoveAt(0);
        Object.Destroy(invocationTransform.GetChild(0).gameObject);

        if (Invocations.Count == 0)
        {
            Invocations.AddRange(DiscardPile);
            DiscardPile.Clear();
            Shuffle();
        }

        CreateCardFactory.Instance.CreateCard(Invocations[0], invocationTransform);
    }
    private void Shuffle()
    {
        //Fisher-Yates
        System.Random r = new();
        int n = Invocations.Count;

        while (n > 1)
        {
            int k = r.Next(n);
            n--;
            (Invocations[n], Invocations[k]) = (Invocations[k], Invocations[n]);
        }
    }
    public bool CanActivate()
    {
        return Invocations[0].effect.CanActivate();
    }
}
