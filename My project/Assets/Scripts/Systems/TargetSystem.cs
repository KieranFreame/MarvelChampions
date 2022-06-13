using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetSystem : MonoBehaviour
{
    #region SingletonPattern
    public static TargetSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        states.Add(new IdleState(this));
        states.Add(new ClearState(this));
        states.Add(new GetCandidatesState(this));
        states.Add(new SelectTargetState(this));

        stateMachine.ChangeState(states[0]);
    }
    #endregion

    #region Pointer
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    void Start()
    {
        raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }
    #endregion

    public StateMachine stateMachine = new StateMachine();
    public List<IState> states = new List<IState>();

    List<dynamic> candidates = new List<dynamic>();
    public dynamic target;
    string targetType;
    string targetTag;

    private void Update()
    {
        stateMachine.Update();
    }

    public IEnumerator GetTarget(string targetTag, string targetComponent)
    {
        this.targetTag = targetTag;
        this.targetType = targetComponent;

        stateMachine.ChangeState(states[1]);

        while (target == null)
            yield return null;
    }

    #region States
    abstract class BaseTargetSystemState : BaseState
    {
        protected TargetSystem owner;
    }

    class IdleState : BaseTargetSystemState
    {
        public IdleState(TargetSystem owner)
        {
            this.owner = owner;
        }
    }

    class ClearState : BaseTargetSystemState
    {
        public ClearState(TargetSystem owner)
        {
            this.owner = owner;
        }

        public override void Enter()
        {
            if (owner.target != null)
                owner.target = null;

            owner.stateMachine.ChangeState(owner.states[2]);
        }
    }

    class GetCandidatesState : BaseTargetSystemState
    {
        public GetCandidatesState(TargetSystem owner)
        {
            this.owner = owner;
        }

        public override void Execute()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag(owner.targetTag))
            {
                owner.candidates.Add(go.GetComponent(owner.targetType));
            }

            owner.stateMachine.ChangeState(owner.states[3]);
        }
    }

    class SelectTargetState : BaseTargetSystemState
    {
        public SelectTargetState(TargetSystem owner)
        {
            this.owner = owner;
        }

        public override void Execute()
        {
            if (Input.GetMouseButton(0))
            {
                owner.pointerEventData = new PointerEventData(owner.eventSystem);
                owner.pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();

                owner.raycaster.Raycast(owner.pointerEventData, results);

                foreach(RaycastResult result in results)
                {
                    if (owner.candidates.Contains(result.gameObject.transform.GetComponent(owner.targetType)))
                    {
                        owner.target = result.gameObject.transform.GetComponent(owner.targetType);
                        owner.stateMachine.ChangeState(owner.states[0]);
                    }
                }
            }
        }
    }
    #endregion
}
