using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSchemeSystem : MonoBehaviour
{
    #region SingletonPattern
    public static TargetSchemeSystem instance;

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

    List<IScheme> schemes = new List<IScheme>();
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
            if (TargetSchemeSystem.instance.target != null)
                TargetSchemeSystem.instance.target = null;

            TargetSchemeSystem.instance.stateMachine.ChangeState(TargetSchemeSystem.instance.states[2]);
        }
    }

    class GetCandidatesState : BaseState
    {
        public override void Execute()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Scheme"))
            {
                if (go.GetComponent<IScheme>() != null)
                    TargetSchemeSystem.instance.schemes.Add(go.GetComponent<IScheme>());
            }

            TargetSchemeSystem.instance.stateMachine.ChangeState(TargetSchemeSystem.instance.states[3]);
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

                    if (TargetSchemeSystem.instance.schemes.Contains(hit.transform.GetComponent<IScheme>()))
                    {
                        TargetSchemeSystem.instance.target = hit.transform.gameObject;
                        TargetSchemeSystem.instance.stateMachine.ChangeState(TargetSchemeSystem.instance.states[0]);
                    }

                }
            }
        }
    }
    #endregion
}
