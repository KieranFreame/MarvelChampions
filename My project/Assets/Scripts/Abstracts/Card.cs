using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public CardData _cd;
    bool faceUp = false;

    public void Flip()
    {
        if (faceUp)
            transform.rotation = new Quaternion(180, 0, 0, 0);
        else
            transform.rotation = Quaternion.identity;
    }
}
