using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public Player firstPlayer { get; set; }
    public int firstPlayerIndex { get; set; }

    private void Start()
    {
        firstPlayerIndex = 0;
        firstPlayer = GameManager.instance.players[firstPlayerIndex];
    }

    public void ChangeFirstPlayer()
    {
        firstPlayerIndex++;

        if (firstPlayerIndex >= GameManager.instance.players.Count)
            firstPlayerIndex = 0;

        firstPlayer = GameManager.instance.players[firstPlayerIndex];
    }
}
