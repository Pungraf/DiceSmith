using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, string> dicesFaces;

    public GameData()
    {
        dicesFaces = new SerializableDictionary<string, string>();
    }
}
