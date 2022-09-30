using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, string> dicesFaces;
    public List<Item> itemList;

    public GameData()
    {
        dicesFaces = new SerializableDictionary<string, string>();
        itemList = new List<Item>();
    }
}
