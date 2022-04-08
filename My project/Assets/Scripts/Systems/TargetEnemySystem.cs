using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemySystem : MonoBehaviour
{
    #region SingletonPattern
    public static TargetEnemySystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        states.Add(new IdleState());
        states.Add(new ClearState());
        states.Add(new GetCandidatesState());
        states.Add(new SelectTargetState());

        stateMachine.ChangeState(states[0]);
    }
    #endregion

    StateMachine stateMachine = new StateMachine();
    List<IState> states = new List<IState>();

    List<IDestructable> minions = new List<IDestructable>();
    public GameObject target = null;

    private void Update()
    {
        stateMachine.Update();
    }

    public IEnumerator GetTarget()
    {
        StartCoroutine(WaitUntilSMFinished());

        while (target == null)
            yield return null;
    }

    IEnumerator WaitUntilSMFinished()
    {
        stateMachine.ChangeState(states[1]);
        yield return new WaitUntil(() => target != null);
    }

    #region States
    class IdleState : BaseState
    {

    }

    class ClearState : BaseState
    {
        public override void Enter()
        {
            if (TargetEnemySystem.instance.target != null)
                TargetEnemySystem.instance.target = null;

            TargetEnemySystem.instance.stateMachine.ChangeState(TargetEnemySystem.instance.states[2]);
        }
    }

    class GetCandidatesState : BaseState
    {
        public override void Execute()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (go.GetComponent<IDestructable>() != null)
                    TargetEnemySystem.instance.minions.Add(go.GetComponent<IDestructable>());
            }

            TargetEnemySystem.instance.stateMachine.ChangeState(TargetEnemySystem.instance.states[3]);
        }
    }

    class SelectTargetState : BaseState
    {
        public override void Execute()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (TargetEnemySystem.instance.minions.Contains(hit.transform.GetComponent<IDestructable>()))
                    {
                        TargetEnemySystem.instance.target = hit.transform.gameObject;
                        TargetEnemySystem.instance.stateMachine.ChangeState(TargetEnemySystem.instance.states[0]);
                    }
                }
            }
        }
    }
    #endregion
}
