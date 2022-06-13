using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyStatusSystem : MonoBehaviour
{
    public static ApplyStatusSystem instance;
    public GameObject statusCard;

    private string targetComp;
    private string targetTag;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        targetComp = string.Empty;
    }

    public void InitiateStatus(ApplyStatusAction action) => StartCoroutine(ApplyStatus(action));

    private IEnumerator ApplyStatus (ApplyStatusAction action)
    {
        if (action.targetSelf) //only apply to allies + minions
        {
            TargetSelf(action);
            yield return null;
        }

        switch (action.status)
        {
            case Status.Stunned:
                targetComp = "Attacker";
                break;
            case Status.Confused:
                if (action.data.cardType == CardType.Ally)
                    targetComp = "Thwarter";
                else
                    targetComp = "Schemer";
                break;
            case Status.Tough:
                targetComp = "Health";
                break;
        }

        //if (action.data is PlayerCard)
            targetTag = "Enemy";
        //else
            //targetTag = "Ally";

        yield return StartCoroutine(TargetSystem.instance.GetTarget(targetTag, targetComp));
        var target = TargetSystem.instance.target;

        ApplyStatusEffect(target);

        yield return null;
    }

    private void TargetSelf(ApplyStatusAction action)
    {
        Transform allies = GameObject.Find("PlayerUI").GetComponent<PlayerTransforms>().alliesTransform;
        int childCount = allies.childCount;

        for (int i = 0; i < childCount; i++)
        {
            if (allies.GetChild(i).GetComponent<CardUI>().card.data == action.data)
            {
                ApplyStatusEffect(allies.GetChild(i), action);
                return;
            }
        }
    }

    private void ApplyStatusEffect(Transform owner, ApplyStatusAction action)
    {
        switch (action.status)
        {
            case Status.Stunned:
                owner.GetComponent<Attacker>().stunned = true;
                break;
            case Status.Confused:
                owner.GetComponent<Thwarter>().confused = true;
                break;
            case Status.Tough:
                owner.GetComponent<Health>().tough = true;
                break;
            default:
                return;
        }
    }

    private void ApplyStatusEffect(dynamic targetComp)
    {
        if (targetComp is Attacker)
        {
            targetComp.stunned = true;
            var inst = Instantiate(GetPrefab("stunned"), targetComp.transform);
        }
        else if (targetComp is Thwarter || targetComp is Schemer)
        {
            targetComp.confused = true;
            var inst = Instantiate(GetPrefab("confused"), targetComp.transform);
        }
        else if (targetComp is Health)
        {
            targetComp.tough = true;
            var inst = Instantiate(GetPrefab("confused"), targetComp.transform);
        }

        return;
    }

    private GameObject GetPrefab(string status)
    {
        GameObject inst = statusCard;

        switch (status)
        {
            case "stunned":
                inst.GetComponent<Image>().color = Color.green;
                inst.GetComponentInChildren<Text>().text = "Stunned";
                break;
            case "confused":
                inst.GetComponent<Image>().color = Color.magenta;
                inst.GetComponentInChildren<Text>().text = "Confused";
                break;
            case "tough":
                inst.GetComponent<Image>().color = Color.yellow;
                inst.GetComponentInChildren<Text>().text = "Tough";
                break;
        }

        return inst;
    }
}
