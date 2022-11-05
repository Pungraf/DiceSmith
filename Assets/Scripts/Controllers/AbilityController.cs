using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbilityController : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private RectTransform abilitySlot;
    [SerializeField]
    private RectTransform abilityPanel;
    [SerializeField]
    private List<string> abilitiesList;

    private List<AbilitySlot> abilitiesSlotsList;

    private List<string> activeAbilities = new List<string>();

    public static AbilityController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

    }

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        InitializeAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToHub()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("HubMenu");
    }

    private void InitializeAbilities()
    {
        AbilitySlot slot;

        foreach (string ability in abilitiesList)
        {
            slot = Instantiate(abilitySlot, abilityPanel).GetComponent<AbilitySlot>();
            if(player.abilitiesList.Contains(ability))
            {
                slot.abilitySelected = true;
            }
            slot.abilityName = ability;
            slot.Configuration();
        }
    }


    public void AddPlayerAbility(string abilityName)
    {
        if(!player.abilitiesList.Contains(abilityName))
        {
            activeAbilities.Add(abilityName);
            player.abilitiesList = activeAbilities;
            DataPersistenceManager.instance.SaveGame();
        }
        else
        {
            Debug.Log("Ability already on list");
        }
    }

    public void RemovePlayerAbility(string abilityName)
    {
        if (player.abilitiesList.Contains(abilityName))
        {
            activeAbilities.Remove(abilityName);
            player.abilitiesList = activeAbilities;
            DataPersistenceManager.instance.SaveGame();
        }
        else
        {
            Debug.Log("Ability not on list");
        }
    }

    public void LoadData(GameData data)
    {
        activeAbilities = data.abilitiesList;
        abilitiesList = data.unlockedAbilitiesList;
    }

    public void SaveData(ref GameData data)
    {
        data.abilitiesList = player.abilitiesList;
        data.unlockedAbilitiesList = abilitiesList;
    }
}
