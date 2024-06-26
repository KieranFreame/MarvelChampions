using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckbuildingPanel : MonoBehaviour
{
    [SerializeField] GameObject HeroSelectionPanel;

    private void OnEnable()
    {
        if (PlayerData.Instance.HeroData == null)
        {
            HeroSelectionPanel.SetActive(true);
        }
    }
}
