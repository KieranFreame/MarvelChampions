using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdentityActions : MonoBehaviour
{
    public Button atk;
    public Button thw;
    public Button rec;
    public Button flip;
    public Player player;

    private void OnEnable()
    {
        atk.gameObject.SetActive(true);
        thw.gameObject.SetActive(true);
        rec.gameObject.SetActive(true);
        flip.gameObject.SetActive(true);

        if (player.identity.hasFlipped)
            flip.gameObject.SetActive(false);

        if (!player.identity.exhausted)
        {
            if (player.identity.activeIdentity == IdentityType.Hero)
            {
                rec.gameObject.SetActive(false);
                return;
            }

            if (player.identity.activeIdentity == IdentityType.AlterEgo)
            {
                atk.gameObject.SetActive(false);
                thw.gameObject.SetActive(false);
                return;
            }
        }

        atk.gameObject.SetActive(false);
        thw.gameObject.SetActive(false);
        rec.gameObject.SetActive(false);
    }

    public void Attack()
    {
        player.transform.Find("HeroInfo").GetComponent<Attacker>().Attack();
        gameObject.SetActive(false);
    }
    public void Thwart()
    {
        player.transform.Find("HeroInfo").GetComponent<Thwarter>().Thwart();
        gameObject.SetActive(false);
    }
    public void Recover()
    {
        player.transform.Find("AlterEgoInfo").GetComponent<Recovery>().Recover();
        gameObject.SetActive(false);
    }
    public void Flip()
    {
        if (player.identity.activeIdentity == IdentityType.AlterEgo)
        {
            player.transform.Find("HeroInfo").gameObject.SetActive(true);
            player.transform.Find("AlterEgoInfo").gameObject.SetActive(false);
        }
        else
        {
            player.transform.Find("HeroInfo").gameObject.SetActive(false);
            player.transform.Find("AlterEgoInfo").gameObject.SetActive(true);
        }

        player.identity.Flip();
        gameObject.SetActive(false);
    }
}
