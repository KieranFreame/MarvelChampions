using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Threading;

public class DefendSystem : MonoBehaviour
{
    public static DefendSystem instance;

    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    #region Events
    public event UnityAction<Player> OnSelectingDefender;
    public event UnityAction OnDefenderSelected;
    #endregion

    #region Fields
    public ICharacter Target { get; set; }
    private readonly List<ICharacter> candidates = new();
    #endregion

    #region Methods

    public async Task<ICharacter> GetDefender(Player targetOwner)
    {
        candidates.Clear();

        OnSelectingDefender?.Invoke(targetOwner);

        Debug.Log("Select Defender");

        candidates.AddRange(targetOwner.CardsInPlay.Allies);

        candidates.RemoveAll(x => x == null);
        candidates.RemoveAll(x => (x as AllyCard).Exhausted);

        if (targetOwner.Identity.ActiveIdentity is Hero && !targetOwner.Identity.Exhausted)
            candidates.Add(targetOwner);

        if (candidates.Count > 0)
        {
            CancellationToken token = CancelButton.ToggleCancelBtn(true, DefenderSelectionCanceled);

            Target = await TargetSystem.instance.SelectTarget(candidates, token:token);

            CancelButton.ToggleCancelBtn(false, DefenderSelectionCanceled);
            OnDefenderSelected?.Invoke();
        }
        else
        {
            Debug.Log("No Defenders Available");
            TargetSystem.SingleTarget(targetOwner);
        }

        if (Target != null)
        {
            if (Target is Player)
            {
                int defence = Target.CharStats.Defender.Defend();
                AttackSystem.instance.Action.Value -= defence;
            }
            else
            {
                (Target as AllyCard).Exhaust();
            }
        }

        return Target;
    }

    private void DefenderSelectionCanceled()
    {
        Target = null;
        CancelButton.ToggleCancelBtn(false, DefenderSelectionCanceled);
    }
    #endregion
}