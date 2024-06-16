using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static InfoPanel instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    [Header("UI Elements")]
    [SerializeField] public GameObject CardInfo;
    [SerializeField] Image CardArt;
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text DescText;
    [SerializeField] Transform CharStats;


    public void OpenPanel(CardData card, PointerEventData eventData, Transform parent)
    {
        NameText.text = card.cardName;
        DescText.text = card.cardDesc;
        CardArt.sprite = card.cardArt;

        if (CharStats != null)
        {
            if (card is AllyCardData)
            {
                CharStats.gameObject.SetActive(true);
                CharStats.Find("Thwart").Find("ThwartText").GetComponent<TMP_Text>().text = (card as AllyCardData).BaseTHW.ToString();
                CharStats.Find("Attack").Find("AttackText").GetComponent<TMP_Text>().text = (card as AllyCardData).BaseATK.ToString();
                CharStats.Find("Health").Find("HealthText").GetComponent<TMP_Text>().text = (card as AllyCardData).BaseHP.ToString();
            }
            else { CharStats.gameObject.SetActive(false); }
        }
        
        if (card is PlayerCardData)
        {
            switch ((card as PlayerCardData).cardAspect)
            {
                case Aspect.Aggression:
                    CardInfo.GetComponent<Image>().color = Color.red;
                    break;
                case Aspect.Justice:
                    CardInfo.GetComponent<Image>().color = Color.yellow;
                    break;
                case Aspect.Leadership:
                    CardInfo.GetComponent<Image>().color = Color.blue;
                    break;
                case Aspect.Protection:
                    CardInfo.GetComponent<Image>().color = Color.green;
                    break;
                default:
                    CardInfo.GetComponent<Image>().color = Color.grey;
                    break;
            }
        }


        CardInfo.SetActive(true);
        transform.position = eventData.position;
    }
    public void OpenPanel(ICharacter character)
    {
        if (character is Player)
        {
            Player p = (Player)character;

            NameText.text = p.Identity.IdentityName;
            DescText.text = p.Identity.ActiveEffect.effectDescription;
        }
        else //villain
        {
            Villain v = (Villain)character;

            NameText.text = v.Name;
            DescText.text = v.VillainEffect.effectDescription;
        }
    }
    public void ClosePanel()
    {
        if (CardInfo != null)
            CardInfo.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CardInfo.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardInfo.SetActive(false);
    }
}
