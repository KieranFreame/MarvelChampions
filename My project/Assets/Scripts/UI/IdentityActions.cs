using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IdentityActions : MonoBehaviour
{
    #region UI Elements
    private Button atk;
    private Button thw;
    private Button rec;
    private Button eff;
    private Button flip;
    private Player player;
    #endregion

    #region Events
    public event UnityAction OnBasicAttack;
    #endregion

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
        if (UIManager.MakingSelection) return;

        atk.gameObject.SetActive(player.Identity.ActiveIdentity is Hero && !player.Identity.Exhausted);
        thw.gameObject.SetActive(player.Identity.ActiveIdentity is Hero && !player.Identity.Exhausted);
        rec.gameObject.SetActive(player.Identity.ActiveIdentity is AlterEgo && player.CharStats.Health.Damaged() && !player.Identity.Exhausted);
        eff.gameObject.SetActive(player.Identity.ActiveEffect.CanActivate());
        flip.gameObject.SetActive(!player.Identity.HasFlipped);
    }

    public async void Attack()
    {
        OnBasicAttack?.Invoke();
        await player.CharStats.InitiateAttack();
        gameObject.SetActive(false);
    }
    public async void Thwart()
    {
        await player.CharStats.InitiateThwart();
        gameObject.SetActive(false);
    }
    public void Recover()
    {
        player.Recover();
        gameObject.SetActive(false);
    }
    public void Flip()
    {
        player.Identity.Flip();
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        player.Identity.Activate();
        gameObject.SetActive(false);
    }
}
