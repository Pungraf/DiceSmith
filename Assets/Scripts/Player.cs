using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{
    public List<Dice> playerDices = new List<Dice>();
    public GameObject dicePrefab;
    public float throwForce = 0f;
    public float dicesOffset = 2f;
    public float holderRotationSpeed = 30f;

    private int dicesOnHand = 0;

    private Dictionary<string, string> facesDictionary = new Dictionary<string, string>();

    public Transform diceHolder;

    [SerializeField]
    private List<string> PlayerDicesNames = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private Vector3 FindeSpawnPoint()
    {
        Vector3 randomPoint = Random.onUnitSphere * 2f  + diceHolder.transform.position;
        return randomPoint;
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

