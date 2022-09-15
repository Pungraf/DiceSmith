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

    private float throwForce = 0f;
    private int dicesOnHand = 0;
    private Dictionary<string, string> facesDictionary = new Dictionary<string, string>();
    
    public List<string> magicTypes = new List<string>();
    public Dictionary<string, int> resourceDictionary = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {

        AssigneMagicTypes();
        AssigneResources();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void AssigneResources()
    {
        foreach (string type in magicTypes)
        {
            resourceDictionary.Add(type + "TierOne", 0);
            resourceDictionary.Add(type + "TierTwo", 0);
            resourceDictionary.Add(type + "TierThree", 0);
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
            if (facesDictionary.TryGetValue(dice.name + "Face_" + (i + 1), out faceName))
            {
                dice.namesList.Add(faceName);
            }
            else
            {
                dice.namesList.Add("");
            }
        }
    }

    public void ThrowDices(Vector3 direction)
    {
        if(direction == Vector3.positiveInfinity)
        {
            return;
        }
        foreach(Dice dice in playerDices)
        {
            if(dice.diceStatus != DiceStatus.InHand)
            {
                continue;
            }
            Vector3 throwDirection = direction - dice.gameObject.transform.position;
            dice.rb.isKinematic = false;
            dice.diceKinematic = false;
            dice.rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
            dice.diceStatus = DiceStatus.OnBoard;
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
    }

    public void SaveData(ref GameData data)
    {
        //Nothing to save now
    }
}

