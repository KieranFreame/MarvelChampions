using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class IdentitySelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    HeroData hData;
    AlterEgoData aEData;

    IdentityPreview preview;

    private void Awake()
    {
        
        preview = GameObject.Find("IdentityDetails").GetComponent<IdentityPreview>();
    }

    public void LoadData(IdentityContainer identity)
    {
        hData = identity.heroData;
        aEData = identity.alterEgoData;

        GetComponent<Image>().sprite = identity.heroData.heroArt;
    }

    public void SelectIdentity()
    {
        PlayerData.Instance.HeroData = hData;
        PlayerData.Instance.AlterEgoData = aEData;
        DeckPreviewPanel.instance.AddHeroCards(aEData.alterEgoName);
        GameObject.Find("HeroSelectionPanel").SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) => preview.ChangePreview(hData, aEData);
    public void OnPointerExit(PointerEventData eventData) => preview.ChangePreview(null, null);
    
}
