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
        dicesFaces = new SerializableDictionary<string, string> {   {"20kOneFaceSlot 1",  "Missfortune"},
                                                                    {"20kOneFaceSlot 20",  "Luck"},
                                                                    {"20kTwoFaceSlot 1",  "Missfortune"},
                                                                    {"20kTwoFaceSlot 20",  "Luck"},
                                                                    {"20kThreeFaceSlot 1",  "Missfortune"},
                                                                    {"20kThreeFaceSlot 20",  "Luck"},
                                                                    {"20kFourFaceSlot 1",  "Missfortune"},
                                                                    {"20kFourFaceSlot 20",  "Luck"},
                                                                    {"20kFiveFaceSlot 1",  "Missfortune"},
                                                                    {"20kFiveFaceSlot 20",  "Luck"}};

        itemList = new List<Item> { new Item { itemType = Item.ItemType.Claw, amount = 15, isStackable = true },
                                    new Item { itemType = Item.ItemType.Blood, amount = 5, isStackable = true }};

        abilitiesList = new List<string>();

        unlockedAbilitiesList = new List<string> {  "BloodDrain",
                                                    "BoneSpear",
                                                    "LunarStrike",
                                                    "BoneShield"};
    }
}

