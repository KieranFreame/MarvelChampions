using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Transform contentTransform;
    [SerializeField] GameObject contentPrefab;

    private void Awake()
    {
        foreach (IdentityContainer i in Database.Instance.identities.database)
        {
            CreateIdentityButton(i);
        }
    }

    void CreateIdentityButton(IdentityContainer i)
    {
        GameObject button = Instantiate(contentPrefab, contentTransform);
        button.GetComponent<IdentitySelect>().LoadData(i);
    }
}
