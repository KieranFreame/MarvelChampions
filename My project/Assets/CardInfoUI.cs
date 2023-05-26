using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoUI : MonoBehaviour
{
    private Transform _panel;
    private TMP_Text _cardNameText;
    private TMP_Text _cardStatsText;
    private TMP_Text _cardDescText;

    private void Awake()
    {
        _panel = transform.GetChild(0);
        _cardNameText = _panel.transform.Find("CardName").GetComponent<TMP_Text>();
        _cardStatsText = _panel.transform.Find("CharStats").GetComponent<TMP_Text>();
        _cardDescText = _panel.transform.Find("CardDesc").GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        UIManager.OnShowCardInfo += ShowCardInfo;
    }

    private void OnDisable()
    {
        UIManager.OnShowCardInfo -= ShowCardInfo;
    }

    private void ShowCardInfo(Card data)
    {

        if (data == null)
        {
            _panel.gameObject.SetActive(false);
            _cardNameText.text = string.Empty;
            _cardStatsText.text = string.Empty;
            _cardDescText.text = string.Empty;
            return;
        }

        _panel.gameObject.SetActive(true);

        _cardNameText.enabled = true;
        _cardNameText.text = data.CardName;

        switch (data.CardType)
        {
            case CardType.Ally:
                _cardStatsText.text = $"" +
                    $"ATK: {data.GetComponent<Attacker>().CurrentAttack}    " +
                    $"THW: {data.GetComponent<Thwarter>().CurrentThwart}    " +
                    $"HP: {data.GetComponent<Health>().CurrentHealth}";
                break;
            case CardType.Minion:
                _cardStatsText.text = $"" +
                    $"ATK: {data.GetComponent<Attacker>().CurrentAttack}    " +
                    $"SCH: {data.GetComponent<Schemer>().CurrentScheme}    " +
                    $"HP: {data.GetComponent<Health>().CurrentHealth}";
                break;
            default:
                _cardStatsText.text = string.Empty;
                break;
        }

        _cardDescText.enabled = true;
        _cardDescText.text = data.CardDesc;
    }
}
