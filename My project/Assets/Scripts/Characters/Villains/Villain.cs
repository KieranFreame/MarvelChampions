using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Villain : MonoBehaviour, IStatus
{
    public int stage;
    protected int[] baseAttack = new int[3];
    public int attack { get; set; }
    protected int[] baseScheme = new int[3];
    public int scheme { get; set; }
    protected int[] baseHP = new int[3];
    public int hp { get; set; }

    #region IStatus
    public bool stunned { get; set; }
    public bool confused { get; set; }
    public bool tough { get; set; }
    #endregion

    public List<IAttachment> attachments = new List<IAttachment>();

    VillainTurnController vtc;

    public virtual void Start()
    {
        vtc = GetComponent<VillainTurnController>();

        switch (stage)
        {
            case 1:
                attack = baseAttack[0];
                scheme = baseScheme[0];
                hp = baseHP[0];
                break;
            case 2:
                attack = baseAttack[1];
                scheme = baseScheme[1];
                hp = baseHP[1];
                break;
            default:
                Debug.Log("Shouldn't be here");
                break;
        }
        
    }

    public void Update()
    {
        if (hp <= 0)
            WhenDefeated();
    }

    public void StartVillainTurn()
    {
        vtc.StartTurn();
    }
    public virtual void Ability() { }
    public virtual void WhenDefeated() { }
    public virtual void Attack(Player player) { }
    public virtual void Scheme() { }
}
