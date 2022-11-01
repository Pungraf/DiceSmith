using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CraftController : MonoBehaviour, IDataPersistence
{
    public static CraftController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

    }

    [SerializeField]
    private List<UI_FaceSlot> faceInputs = new List<UI_FaceSlot>(18);
    [SerializeField]
    private MoveAroundObject cameraTarget;
    [SerializeField]
    private GameObject dicePrefab;
    [SerializeField]
    private Player player;

    private string activeDice = "";
    private Dice dice;

    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = player.inventory;
        InstantiateDice("20kOne");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToHub()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(1);
    }

    private void ClearFaceSlots()
    {
        foreach( UI_FaceSlot faceSlot in faceInputs)
        {
            Item empty = new Item { itemType = Item.ItemType.Empty, amount = 1, isStackable = false };
            faceSlot.SetItem(empty);
            faceSlot.SetSprite(empty.GetSprite());
        }
    }

    // Spawn and populate saved faces in dice by name
    public void InstantiateDice(string diceName)
    {
        if(dice != null)
        {
            Destroy(dice.gameObject);
        }
        GameObject goDice =  Instantiate(dicePrefab);
        goDice.name = diceName;
        dice = goDice.GetComponent<Dice>();
        activeDice = diceName;
        ClearFaceSlots();
        DataPersistenceManager.instance.LoadGame();
        GenerateDiceFaces();
        cameraTarget._target = goDice.transform;
    }


    void populateFaceNames(Dice dice)
    {
        dice.namesList.Clear();
        dice.namesList.Add("Missfortune");
        for (int i = 0; i < 18; i++)
        {
            dice.namesList.Add(faceInputs[i].GetItem().itemType.ToString());
        }
        dice.namesList.Add("Luck");
    }

    public void GenerateDiceFaces()
    {
        if(dice is null)
        {
            Debug.Log("No dice to Craft");
            return;
        }
        populateFaceNames(dice);
        dice.GenerateFaces();
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        string faceType;
        Item.ItemType itemType;
        foreach (UI_FaceSlot field in faceInputs)
        {
            if(field.GetItem() == null)
            {
                return;
            }
            data.dicesFaces.TryGetValue(activeDice + field.name, out faceType);
            if(faceType == null)
            {
                itemType = Item.ItemType.Empty;
            }
            else
            {
                itemType = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), faceType);
            }
            field.SetItem(new Item { itemType = itemType, amount = 1, isStackable = true });
            field.SetSprite(Item.GetSprite(field.GetItem().itemType));
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.dicesFaces.ContainsKey(activeDice + "FaceSlot 1"))
        {
            data.dicesFaces.Remove(activeDice + "FaceSlot 1");
        }
        data.dicesFaces.Add(activeDice + "FaceSlot 1", "Missfortune");
        foreach (UI_FaceSlot field in faceInputs)
        {
            if (data.dicesFaces.ContainsKey(activeDice + field.name))
            {
                data.dicesFaces.Remove(activeDice + field.name);
            }
            data.dicesFaces.Add(activeDice + field.name, field.GetItem().itemType.ToString());
        }
        if (data.dicesFaces.ContainsKey(activeDice + "FaceSlot 20"))
        {
            data.dicesFaces.Remove(activeDice + "FaceSlot 20");
        }
        data.dicesFaces.Add(activeDice + "FaceSlot 20", "Luck");
    }
}
