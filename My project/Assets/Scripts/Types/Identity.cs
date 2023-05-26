using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class Identity : IAttached, IExhaust
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
    public event UnityAction SetupComplete;
    #endregion

    public Identity(Player p, HeroData h, AlterEgoData a)
    { 
        Hero = new Hero(h, p);
        AlterEgo = new AlterEgo(a, p);

        ActiveIdentity = AlterEgo;
        ActiveEffect = AlterEgo.Effect;
           
        animator = p.transform.Find("HeroIdentityProfile").GetComponent<Animator>();
        heroUI = p.GetComponentInChildren<HeroUI>(true);
        alterEgoUI = p.GetComponentInChildren<AlterEgoUI>();

        IdentityActions.Flipping += Flip;
        IdentityActions.Activating += Activate;
        TurnManager.OnEndPlayerPhase += EndPlayerPhase;

        SetupComplete?.Invoke();
        alterEgoUI.LoadUI(this);
    }
    protected virtual void OnDisable()
    {
        IdentityActions.Flipping -= Flip;
        IdentityActions.Activating -= Activate;
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
    private void FlipToHero()
    {
        ActiveIdentity = Hero;
        IdentityName = Hero.Name;

        IdentityTraits.Clear();

        foreach (string trait in Hero.Traits)
            IdentityTraits.Add(trait);
        
        alterEgoUI.gameObject.SetActive(false);
        heroUI.gameObject.SetActive(true);

        ActiveEffect = Hero.Effect;

        FlippedToHero?.Invoke();
    }
    private void FlipToAlterEgo()
    {
        ActiveIdentity = AlterEgo;
        IdentityName = AlterEgo.Name;

        IdentityTraits.Clear();

        foreach (string trait in AlterEgo.Traits)
        {
            IdentityTraits.Add(trait);
        }
            
        alterEgoUI.gameObject.SetActive(true);
        heroUI.gameObject.SetActive(false);

        ActiveEffect = AlterEgo.Effect;

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

    #region Testing
    #endregion
}
