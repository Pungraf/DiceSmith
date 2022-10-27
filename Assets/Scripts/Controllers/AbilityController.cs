using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private RectTransform abilitySlot;
    [SerializeField]
    private RectTransform abilityPanel;

    public static AbilityController Instance { get; private set; }
    public List<string> activeAbilities = new List<string>();

    private void Awake()
    {
        Instance = this;

    }

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        InitializeActiveAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeActiveAbilities()
    {
        AbilitySlot slot;

        foreach (string ability in activeAbilities)
        {
            slot = Instantiate(abilitySlot, abilityPanel).GetComponent<AbilitySlot>();
            slot.abilitySelected = true;
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
    }

    public void SaveData(ref GameData data)
    {
        data.abilitiesList = player.abilitiesList;
    }
}