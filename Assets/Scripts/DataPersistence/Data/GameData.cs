using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, string> dicesFaces;
    public List<Item> itemList;
    public List<string> abilitiesList;
    public List<string> unlockedAbilitiesList;

    public GameData()
    {
        dicesFaces = new SerializableDictionary<string, string>();
        itemList = new List<Item>();
        abilitiesList = new List<string>();
        unlockedAbilitiesList = new List<string>();
    }
}

