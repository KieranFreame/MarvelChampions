using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Identity Database", menuName = "MarvelChampions/Databases/Identity Database")]
public class IdentityDatabase : ScriptableObject
{
    public List<IdentityContainer> database = new List<IdentityContainer>();
}

[System.Serializable]
public struct IdentityContainer
{
    public HeroData heroData;
    public AlterEgoData alterEgoData;
}
