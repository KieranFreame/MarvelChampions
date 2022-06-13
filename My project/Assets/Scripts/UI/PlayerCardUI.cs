using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardUI : CardUI
{
    public Text cardCostText;
    public Image cardBase;
    public Image resource;

    Dictionary<string, Color> cardColors = new Dictionary<string, Color>()
    {
        {"justice", Color.yellow },
        {"aggression", Color.red },
        {"protection", Color.green },
        {"leadership", Color.blue },
        {"basic", Color.grey },
    };

    Dictionary<Resource, Color> resourceColor = new Dictionary<Resource, Color>() 
    {
        {Resource.Wild, Color.green },
        {Resource.Physical, Color.red },
        {Resource.Scientific, Color.blue },
        {Resource.Energy, Color.yellow },
    };

    // Start is called before the first frame update
    protected override void Start()
    {
        if (card.data != null)
            StartCoroutine(LoadData());
    }

    protected override IEnumerator LoadData()
    {
        yield return StartCoroutine(base.LoadData());

        if (cardCostText != null)
            cardCostText.text = (card.data as PlayerCard).cardCost.ToString();

        if (cardColors.ContainsKey((card.data as PlayerCard).aspect.ToLower()))
            cardBase.color = cardColors[(card.data as PlayerCard).aspect.ToLower()];
        else
            cardBase.color = Color.magenta;

        resource.color = resourceColor[(card.data as PlayerCard).resources[0]];
    }

    public override void Refresh()
    {
        base.Refresh();
        if (cardCostText != null)
            cardCostText.text = (card.data as PlayerCard).cardCost.ToString();
    }
}
