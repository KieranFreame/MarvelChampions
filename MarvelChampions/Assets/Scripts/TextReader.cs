using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextReader
{
    public static List<CardData> PopulateDeck(string filename)
    {
        string path = "Assets/CardLists/" + filename;

        List<CardData> deckToReturn = new();

        foreach (string line in File.ReadAllLines(path))
        {
            if (Database.GetCardDataById(line) == null)
                Debug.Log(line + " is not in the database");

            deckToReturn.Add(Database.GetCardDataById(line));
        }

        return deckToReturn;
    }
}
