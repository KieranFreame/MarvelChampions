using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerData
{
    private static PlayerData instance;
    
    public static PlayerData Instance
    {
        get
        {
            instance ??= new PlayerData();
            return instance;
        }
    }

    public List<CardData> PlayerDeck { get; set; }
    public HeroData HeroData { get; set; }
    public AlterEgoData AlterEgoData { get; set; }

    PlayerData()
    {
        PlayerDeck = new List<CardData>(50);
    }

    public void MoveScenes()
    {
        PlayerDeck = DeckPreviewPanel.playerDeck.ToList<CardData>();

        if (HeroData == null || AlterEgoData == null)
        {
            Debug.Log("Cannot start: Player has not selected an identity");
            return;
        }

        if (PlayerDeck.Count < 40 || PlayerDeck.Count >= 50)
        {
            Debug.Log($"Cannot start: Player deck does not meet deckbuilding restrictions");
            return;
        }

        if (ScenarioManager.inst.villain == null)
        {
            Debug.Log("Cannot start: A villain has not been selected");
            return;
        }

        if (ScenarioManager.inst.EncounterSets.Count == 0)
        {
            Debug.Log("Cannot start: At least 1 encounter set must be included");
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("GameBoardScene");
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Player p = Object.FindObjectOfType<Player>();

        p.LoadData(HeroData, AlterEgoData, PlayerDeck);

        ScenarioManager.inst.OnSceneLoaded(arg0, arg1);
    }
}
