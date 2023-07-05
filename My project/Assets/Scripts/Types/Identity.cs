using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class Identity : IExhaust
{
    //Identity Parameters
    [SerializeField] private HeroData _HeroData;
    [SerializeField] private AlterEgoData _AlterEgoData;

    #region Properties 
    public Hero Hero { get; set; }
    public AlterEgo AlterEgo { get; set; }
    public dynamic ActiveIdentity { get; set; }
    public string IdentityName { get; protected set; }
    public ObservableCollection<string> IdentityTraits { get; protected set; } = new ObservableCollection<string>();
    public List<Keywords> Keywords { get; } = new List<Keywords>();
    public int BaseHP { get => AlterEgo.BaseHP; }
    public bool HasFlipped { get; set; } = false;
    public bool Exhausted { get; set; } = false;
    public List<IAttachment> Attachments { get; } = new List<IAttachment>();
    public IdentityEffect ActiveEffect { get; private set; }
    #endregion

    private readonly Animator animator;
    private readonly HeroUI heroUI;
    private readonly AlterEgoUI alterEgoUI;

    #region Events
    public event UnityAction FlippedToHero;
    public event UnityAction FlippedToAlterEgo;
    #endregion

    public Identity(Player p, HeroData h, AlterEgoData a)
    { 
        Hero = new Hero(h, p);
        AlterEgo = new AlterEgo(a, p);

        ActiveIdentity = AlterEgo;
        ActiveEffect = AlterEgo.Effect;
           
        animator = p.GetComponent<Animator>();
        heroUI = p.transform.parent.Find("HeroInfo").GetComponent<HeroUI>();
        alterEgoUI = p.transform.parent.Find("AlterEgoInfo").GetComponent<AlterEgoUI>();

        TurnManager.OnEndPlayerPhase += EndPlayerPhase;
    }
    protected virtual void OnDisable()
    {
        TurnManager.OnEndPlayerPhase -= EndPlayerPhase;
    }

    #region Flipping
    public void Flip()
    {
        if (ActiveIdentity is Hero)
            FlipToAlterEgo();
        else
            FlipToHero();

        HasFlipped = true;
    }
    public void FlipToHero()
    {
        ActiveIdentity = Hero;
        IdentityName = Hero.Name;

        IdentityTraits.ToList().RemoveAll(x => AlterEgo.Traits.Contains(x));

        foreach (string trait in Hero.Traits)
            IdentityTraits.Add(trait);
        
        alterEgoUI.gameObject.SetActive(false);
        heroUI.gameObject.SetActive(true);

        ActiveEffect.OnFlipDown();
        ActiveEffect = Hero.Effect;
        ActiveEffect.OnFlipUp();

        FlippedToHero?.Invoke();
    }
    public void FlipToAlterEgo()
    {
        ActiveIdentity = AlterEgo;
        IdentityName = AlterEgo.Name;

        IdentityTraits.ToList().RemoveAll(x => Hero.Traits.Contains(x));

        foreach (string trait in AlterEgo.Traits)
        {
            IdentityTraits.Add(trait);
        }
            
        alterEgoUI.gameObject.SetActive(true);
        heroUI.gameObject.SetActive(false);

        ActiveEffect.OnFlipDown();
        ActiveEffect = AlterEgo.Effect;
        ActiveEffect.OnFlipUp();

        FlippedToAlterEgo?.Invoke();
    }
    #endregion

    public void Activate()
    {
        ActiveEffect.Activate(); 
    }
    public void EndPlayerPhase()
    {
        HasFlipped = false;
        Ready();
    }
    public void Ready()
    {
        if (Exhausted)
        {
            animator.Play("IdentityReady");
            Exhausted = false;
        }
    }
    public void Exhaust()
    {
        if (!Exhausted)
        {
            animator.Play("IdentityExhaust");
            Exhausted = true;
        }
    }
}
