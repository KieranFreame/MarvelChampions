using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static AttackSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    #region GameEvents
    [SerializeField]
    private GameEvent attackInitiated;
    [SerializeField]
    private GameEvent attackComplete;
    [SerializeField]
    private GameEvent applyingOverkill;
    [SerializeField]
    private GameEvent overkillApplied;
    #endregion
    #region Fields
    ICombatant attacker;
    IDestructable target;
    List<string> keywords;
    public int excess = 0;
    #endregion
    #region Properties
    public ICombatant Attacker
    {
        get
        {
            return attacker;
        }
    }
    public IDestructable Target
    {
        get
        {
            return target;
        }
    }
    public List<string> Keywords
    {
        get
        {
            return keywords;
        }
    }
    #endregion

    public void Attack(AttackAction attack)
    {
        attacker = attack.attacker;
        target = attack.target;
        keywords = attack.keywords;

        //Spider-Man + Defence
        attackInitiated.Raise();

        if (keywords.Contains("Piercing"))
            (target as IStatus).tough = false;

        if (keywords.Contains("Overkill") && target.GetType() != typeof(Villain))
            excess = attacker.attack - target.hitpoints.currHealth;

        (target as MonoBehaviour).gameObject.SendMessage("TakeDamage", attacker.attack - excess, SendMessageOptions.DontRequireReceiver);

        if (excess > 0 && GameManager.instance.scenario.villain != null)
        {
            applyingOverkill.Raise();
            (GameManager.instance.scenario.villain as MonoBehaviour).gameObject.SendMessage("TakeDamage", excess, SendMessageOptions.DontRequireReceiver);
            overkillApplied.Raise();
        }

        attackComplete.Raise();

        attacker = null;
        target = null;
        keywords.Clear();
        excess = 0;
        return;
    }
}
