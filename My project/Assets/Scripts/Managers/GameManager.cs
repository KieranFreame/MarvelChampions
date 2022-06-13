using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Player> players { get; set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        players = new List<Player>();
        players.AddRange(GameObject.FindObjectsOfType<Player>());
    }
}
