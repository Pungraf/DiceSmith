using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftController : MonoBehaviour, IDataPersistence
{
    public static CraftController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private List<TMP_InputField> faceInputs = new List<TMP_InputField>(20);
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
        DataPersistenceManager.instance.LoadGame();
        inventory = player.inventory;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        DataPersistenceManager.instance.LoadGame();
        GenerateDiceFaces();
        cameraTarget._target = goDice.transform;
    }


    void populateFaceNames(Dice dice)
    {
        dice.namesList.Clear();
        for(int i = 0; i < 20; i++)
        {
            dice.namesList.Add(faceInputs[i].text.ToString());
        }
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
        foreach (TMP_InputField field in faceInputs)
        {
            data.dicesFaces.TryGetValue(activeDice + field.name, out faceType);
            field.text = faceType;
        }
    }

    public void SaveData(ref GameData data)
    {
        foreach (TMP_InputField field in faceInputs)
        {
            if (data.dicesFaces.ContainsKey(activeDice + field.name))
            {
                data.dicesFaces.Remove(activeDice + field.name);
            }
            data.dicesFaces.Add(activeDice + field.name, field.text.ToString());
        }
    }
}
