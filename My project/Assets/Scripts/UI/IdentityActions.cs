using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IdentityActions : MonoBehaviour
{
    private Button atk;
    private Button thw;
    private Button rec;
    private Button eff;
    private Button flip;
    private Player player;

    //Events
    public static event UnityAction Activating;
    public static event UnityAction Flipping;

    private void Awake()
    {
        atk = transform.Find("Attack").gameObject.GetComponent<Button>();
        thw = transform.Find("Thwart").gameObject.GetComponent<Button>();
        rec = transform.Find("Recover").gameObject.GetComponent<Button>();
        eff = transform.Find("Activate").gameObject.GetComponent<Button>();
        flip = transform.Find("Flip").gameObject.GetComponent<Button>();

        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        if (UIManager.InStateMachine) return;

        atk.gameObject.SetActive(player.Identity.ActiveIdentity is Hero && !player.Identity.Exhausted);
        thw.gameObject.SetActive(player.Identity.ActiveIdentity is Hero && !player.Identity.Exhausted);
        rec.gameObject.SetActive(player.Identity.ActiveIdentity is AlterEgo && player.CharStats.Health.Damaged() && !player.Identity.Exhausted);
        eff.gameObject.SetActive(player.Identity.ActiveEffect.CanActivate());
        flip.gameObject.SetActive(!player.Identity.HasFlipped);
    }

    public void Attack()
    {
        StartCoroutine(player.CharStats.InitiateAttack());
        gameObject.SetActive(false);
    }
    public void Thwart()
    {
        StartCoroutine(player.CharStats.InitiateThwart());
        gameObject.SetActive(false);
    }
    public void Recover()
    {
        player.CharStats.InitiateRecover();
        gameObject.SetActive(false);
    }
    public void Flip()
    {
        Flipping?.Invoke();
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        Activating?.Invoke();
        gameObject.SetActive(false);
    }
}
