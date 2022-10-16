using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Entity, IDataPersistence
{
    public List<Dice> playerDices = new List<Dice>();

    [SerializeField]
    private float dicesOffset = 2f;
    [SerializeField]
    private GameObject dicePrefab;
    [SerializeField]
    private Transform diceHolder;
    [SerializeField]
    private List<string> PlayerDicesNames = new List<string>();
    [SerializeField]
    private UI_Inventory UI_Inventory;

    private float throwForce = 0f;
    private int dicesOnHand = 0;
    private Dictionary<string, string> facesDictionary = new Dictionary<string, string>();

    public Inventory inventory;
    public List<string> magicTypes = new List<string>();
    public Dictionary<string, int> resourceDictionary = new Dictionary<string, int>();
    public Animator animator;
    public Vector3 playerThrowDirection = new Vector3();

    private void Awake()
    {
        inventory = new Inventory();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(UI_Inventory != null)
        {
            UI_Inventory.SetInventory(inventory);
        }
        AssigneMagicTypes();
        AssigneResources();
    }

    // Update is called once per frame
    void Update()
    {
        // Spawning items for testing
        //Nigh
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.J))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.Eclipse, amount = 1, isStackable = true });
        }
        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.J))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.Crescent, amount = 1, isStackable = true });
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.J))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.FullMoon, amount = 1, isStackable = true });
        }
        //Blood
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.K))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.Blood, amount = 1, isStackable = true });
        }
        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.K))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.Veins, amount = 1, isStackable = true });
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.K))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.Heart, amount = 1, isStackable = true });
        }
        //Bone
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.Claw, amount = 1, isStackable = true });
        }
        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.Bone, amount = 1, isStackable = true });
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.Skull, amount = 1, isStackable = true });
        }
    }

    private void AssigneResources()
    {
        foreach (string type in magicTypes)
        {
            resourceDictionary.Add(type + "Tier1", 0);
            resourceDictionary.Add(type + "Tier2", 0);
            resourceDictionary.Add(type + "Tier3", 0);
        }
    }

    private void AssigneMagicTypes()
    {
        string type = "";
        string magicType = "";
        foreach (KeyValuePair<string, string> entry in facesDictionary)
        {
            type = entry.Value;
            if (type == "Blood" || type == "Veins" || type == "Heart")
            {
                magicType = "Blood";
            }
            else if(type == "Claw" || type == "Bone" || type == "Skull")
            {
                magicType = "Bone";
            }
            else if (type == "Eclipse" || type == "Crescent" || type == "FullMoon")
            {
                magicType = "Night";
            }

            if (magicTypes.Contains(magicType) || magicType == "")
            {
                continue;
            }
            else
            {
                magicTypes.Add(magicType);
            }
        }
    }

    public void InstantiateDicesToReroll(List<string> dicesToReroll)
    {
        dicesOnHand = 0;
        foreach (string diceName in dicesToReroll)
        {
            InstantiatePlayerDice(diceName);
            dicesOnHand++;
        }

        SetDicesPosition(dicesOnHand);
    }

    public void InstantiateAllPlayerDices()
    {
        dicesOnHand = 0;
        foreach (string diceName in PlayerDicesNames)
        {
            InstantiatePlayerDice(diceName);
            dicesOnHand++;
        }

        SetDicesPosition(dicesOnHand);
    }

    private void InstantiatePlayerDice(string diceName)
    {
        Dice dice = null;
        dice = playerDices.Where(obj => obj.name == diceName).SingleOrDefault();
        if (dice != null)
        {
            Destroy(dice.gameObject);
            playerDices.RemoveAll(item => item.name == dice.name);
        }
        GameObject goDice = Instantiate(dicePrefab);
        goDice.layer = LayerMask.NameToLayer("No Outline");
        goDice.name = diceName;
        dice = goDice.GetComponent<Dice>();
        DataPersistenceManager.instance.LoadGame();
        GenerateDiceFaces(dice);
        playerDices.Add(dice);
        dice.diceStatus = DiceStatus.InHand;
        goDice.transform.SetParent(diceHolder);
    }

    private void SetDicesPosition(int numberOfDices)
    {
        int diceNumber = 0;

        Vector3[] oneDIce = new Vector3[] {new Vector3(0f, 0f, 0.1f)};

        Vector3[] TwoDIce = new Vector3[] { new Vector3(0f, 0f, 0.1f),
                                            new Vector3(0f, 0f, -0.1f)};

        Vector3[] threeDIce = new Vector3[] {new Vector3(0f, 0.1f, 0f),
                                            new Vector3(0.0866025f, -0.05f, 0f),
                                            new Vector3(-0.0866025f, -0.05f, 0f)};

        Vector3[] fourDIce = new Vector3[] {new Vector3(0f, 0f, 0.1f),
                                            new Vector3(-0.04714f, 0.08165f, -0.033333f),
                                            new Vector3(0.094281f, 0f, -0.033333f),
                                            new Vector3(-0.04714f, -0.08165f, -0.033333f) };

        Vector3[] fiveDIce = new Vector3[] {new Vector3(0f, 0.1f, 0f),
                                            new Vector3(0.0866025f, -0.05f, 0f),
                                            new Vector3(-0.0866025f, -0.05f, 0f),
                                            new Vector3(0f, 0f, 0.1f),
                                            new Vector3(0f, 0f, -0.1f)};

        switch (numberOfDices)
        {
            case 1:
                foreach (Dice dice in playerDices)
                {
                    if (dice.diceStatus == DiceStatus.InHand)
                    {
                        dice.gameObject.transform.position = oneDIce[diceNumber] * dicesOffset + diceHolder.position;
                        dice.posOffset = dice.transform.position;
                        diceNumber++;
                    }
                }
                break;
            case 2:
                foreach (Dice dice in playerDices)
                {
                    if (dice.diceStatus == DiceStatus.InHand)
                    {
                        dice.gameObject.transform.position = TwoDIce[diceNumber] * dicesOffset + diceHolder.position;
                        dice.posOffset = dice.transform.position;
                        diceNumber++;
                    }
                }
                break;
            case 3:
                foreach (Dice dice in playerDices)
                {
                    if (dice.diceStatus == DiceStatus.InHand)
                    {
                        dice.gameObject.transform.position = threeDIce[diceNumber] * dicesOffset + diceHolder.position;
                        dice.posOffset = dice.transform.position;
                        diceNumber++;
                    }
                }
                break;
            case 4:
                foreach(Dice dice in playerDices)
                {
                    if(dice.diceStatus == DiceStatus.InHand)
                    {
                        dice.gameObject.transform.position = fourDIce[diceNumber] * dicesOffset + diceHolder.position;
                        dice.posOffset = dice.transform.position;
                        diceNumber++;
                    }
                }
                break;
            case 5:
                foreach (Dice dice in playerDices)
                {
                    if (dice.diceStatus == DiceStatus.InHand)
                    {
                        dice.gameObject.transform.position = fiveDIce[diceNumber] * dicesOffset + diceHolder.position;
                        dice.posOffset = dice.transform.position;
                        diceNumber++;
                    }
                }
                break;
        }
    }

    private void GenerateDiceFaces(Dice dice)
    {
        if (dice is null)
        {
            Debug.Log("No dice to Craft");
            return;
        }
        populateFaceNames(dice);
        dice.GenerateFaces();
    }

    private void populateFaceNames(Dice dice)
    {
        dice.namesList.Clear();
        string faceName = "";

        for (int i = 0; i < 20; i++)
        {
            if (facesDictionary.TryGetValue(dice.name + "FaceSlot " + (i + 1), out faceName))
            {
                dice.namesList.Add(faceName);
            }
            else
            {
                dice.namesList.Add("Empty");
            }
        }
    }

    public void AnimationThrowDice()
    {
        GameController.Instance.ThrowPlayerDices();
    }

    public void ThrowDices()
    {
        if(playerThrowDirection == Vector3.positiveInfinity)
        {
            return;
        }
        foreach(Dice dice in playerDices)
        {
            if(dice.diceStatus != DiceStatus.InHand)
            {
                continue;
            }
            Vector3 throwDirection = playerThrowDirection - dice.gameObject.transform.position;
            dice.rb.isKinematic = false;
            dice.diceKinematic = false;
            dice.rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
            dice.diceStatus = DiceStatus.OnBoard;
            diceHolder.DetachChildren();
        }
    }

    public void SetForce(System.Single force)
    {
        throwForce = force;
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, string> face in data.dicesFaces)
        {
            if (facesDictionary.ContainsKey(face.Key))
            {
                facesDictionary.Remove(face.Key);
            }
            facesDictionary.Add(face.Key, face.Value);
        }

        inventory.SetItemList(data.itemList);
    }

    public void SaveData(ref GameData data)
    {
        data.itemList = inventory.GetItemList();
    }
}

