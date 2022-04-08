using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static ClickManager instance;
    [SerializeField]
    public GameObject clickedObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        clickedObject = this.gameObject;
    }

    public void Update()
    {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    clickedObject = hit.transform.gameObject; 
                }
            }
    }
}
