using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MarvelChampions/Card Set", fileName = "New Card Set")]
public class CardSet : ScriptableObject
{
    public List<string> cardIDs;
}
