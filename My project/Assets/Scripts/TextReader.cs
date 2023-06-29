using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextReader
{
    public static List<CardData> PopulateDeck(string filename)
    {
        string path = "Assets/Decklists/" + filename;

        List<CardData> deckToReturn = new();
        List<string> lines = new(); 
        lines.AddRange(File.ReadAllLines(path));

        foreach (string line in lines)
        {
            deckToReturn.Add(Database.GetCardDataById(line));
        }

        return deckToReturn;
    }
}
