using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour, IDataPersistence
{
    public static GameController Instance { get; private set; }

    

    public int rerolls;

    [SerializeField]
    private bool dicesRolled;
    [SerializeField]
    private bool dicesInMove;
    [SerializeField]
    private int resourceCapacity;
    [SerializeField]
    private TMP_Text playerHealth;
    [SerializeField]
    private TMP_Text enemyHealth;
    [SerializeField]
    private Camera eyeCamera;
    [SerializeField]
    private GameObject eyeGo;
    [SerializeField]
    private Transform eyeCameraClampObject;
    [SerializeField]
    private float cameraClampValue;

    //Hardcoded place to spawn enemy
    [SerializeField]
    private Vector3 enemySpawnPoint = new Vector3(0f, 5.5f, 35f);

    // TODO fix empty objects in goDices and Dices ( now fixed by null checkup in methods )
    private GameObject[] goDices;
    private List<string> dicesToReroll;
    private List<Dice> Dices = new List<Dice>();
    private Vector3 throwDirection;
    private Coroutine rollCoroutine;
    private bool onEncounter;
    private bool rolled;
    private bool resourcesAssigned;
    private bool statusCheck;
    private bool lookupMode;
    private bool backToCameraOrigin;
    private bool freeCameraMovement;
    private Vector3 cameraTargetLocation;
    private Vector3 cameraTargetRotation;
    private float cameraSpeed = 5f;
    private float cameraRotationSpeed = 5f;
    private float xAxisCameraMovement;
    private float zAxisCameraMovement;
    private string entityTurn;
    


    public TurnPhase turnPhase;
    public bool playerTurn;
    public string lootToText;
    public Player player;
    public Enemy enemy;

    [SerializeField]
    private TMP_Text gamePhaseText;
    [SerializeField]
    private RectTransform abilitiesPanel;
    [SerializeField]
    private RectTransform resourcesMainPanel;
    [SerializeField]
    private RectTransform temporalResourcePanel;
    [SerializeField]
    private GameObject resourcesPanelPrefab;
    [SerializeField]
    private RectTransform finishPanel;
    [SerializeField]
    private TMP_Text lootText;

    private void Awake()
    {
        Instance = this;

        if(MultiSceneVariables.ArenaEnemyToSpawn != null)
        {
            Instantiate(Resources.Load("Enemies/" + MultiSceneVariables.ArenaEnemyToSpawn), enemySpawnPoint, Quaternion.identity);
        }
        else
        {
            Instantiate(Resources.Load("Enemies/Jailer"), enemySpawnPoint, Quaternion.identity);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        dicesToReroll = new List<string>();
        dicesRolled = false;
        dicesInMove = false;
        FindeDices();
        player.SetEnemy(enemy);
        enemy.SetEnemy(player);

        foreach(string magicType in player.magicTypes)
        {
            GameObject panel =  Instantiate(resourcesPanelPrefab);
            panel.transform.SetParent(resourcesMainPanel, false);
            panel.name = magicType + "Panel";
            Transform tierPanel;
            GameObject token;

            Transform transormPanel = panel.transform;

            for (int i = 0; i < 3; i++)
            {
                int tier = i + 1;
                tierPanel = transormPanel.Find("SingleResource" + tier + "Tier");
                for(int j = 0; j < 5; j++)
                {
                    token = (GameObject)Instantiate(Resources.Load("UiTokens/" + magicType + tier));
                    token.transform.SetParent(tierPanel, false);
                    token.SetActive(false);
                }
            }
        }


        player.Health = 20;

        playerHealth.text = player.Health.ToString();
        enemyHealth.text = enemy.Health.ToString();

        AssigneAbilities();

        StartCoroutine(Encounter());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();

        if (lookupMode)
        {
            if (eyeCamera.transform.position == cameraTargetLocation)
            {
                freeCameraMovement = true;
            }
            if(!freeCameraMovement)
            {
                eyeCamera.transform.position = Vector3.MoveTowards(eyeCamera.transform.position, cameraTargetLocation, cameraSpeed * Time.deltaTime);
                eyeCamera.transform.eulerAngles = Vector3.Lerp(eyeCamera.transform.eulerAngles, cameraTargetRotation, cameraRotationSpeed * Time.deltaTime);
            }
        }
        if(backToCameraOrigin && eyeCamera.transform.position == cameraTargetLocation)
        {
            lookupMode = false;
            backToCameraOrigin = false;
        }

        if(freeCameraMovement)
        {
            CameraLookupMovement();
        }

        //Player input
        if(onEncounter && playerTurn && turnPhase == TurnPhase.Main)
        {
            if (Input.GetMouseButtonDown(0) && GetWorldPoint() != Vector3.zero && !dicesInMove && !dicesRolled)
            {
                player.animator.SetTrigger("Throw");
            }
    
            if (Input.GetMouseButtonDown(1) && GetWorldPoint() != Vector3.zero && !dicesInMove && dicesRolled)
            {
                addDiceToReroll();
            }
        }

        //Check if encounter is finished
        if(player.Health <= 0 || enemy.Health <= 0)
        {
            onEncounter = false;
        }
    }

    // Returnto hub scene
    public void BackToHub()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("HubMenu");
    }

    //Camer movement during lookup mode
    public void CameraLookupMovement()
    {
        if(Input.GetKey(KeyCode.W) && (eyeCamera.transform.position.z - eyeCameraClampObject.position.z) <= cameraClampValue)
        {
            eyeCamera.transform.Translate(Vector3.up * Time.deltaTime * cameraSpeed);
        }
        if (Input.GetKey(KeyCode.S) && (eyeCameraClampObject.position.z - eyeCamera.transform.position.z) <= cameraClampValue)
        {
            eyeCamera.transform.Translate(Vector3.down * Time.deltaTime * cameraSpeed);
        }
        if (Input.GetKey(KeyCode.A) && (eyeCameraClampObject.position.x - eyeCamera.transform.position.x) <= cameraClampValue)
        {
            eyeCamera.transform.Translate(Vector3.left * Time.deltaTime * cameraSpeed);
        }
        if (Input.GetKey(KeyCode.D) && (eyeCamera.transform.position.x - eyeCameraClampObject.position.x) <= cameraClampValue)
        {
            eyeCamera.transform.Translate(Vector3.right * Time.deltaTime * cameraSpeed);
        }
    }


    // Surrender current fight
    public void Surrender()
    {
        player.Health = 0;
    }

    public void NextPhase()
    {
        if(!playerTurn || (playerTurn && turnPhase == TurnPhase.Main && !dicesRolled))
        {
            return;
        }
        turnPhase = turnPhase.Next();
    }

    //Finish current turn
    public void FinishTurn()
    {
        if(!dicesRolled)
        {
            Debug.Log("Roll dices befor finishing turn");
            return;
        }
        if(!resourcesAssigned)
        {
            AssigneResources();
        }
        ExitLookupMode();
        ClearTemporalPanel();
        playerTurn = false;

        InstantiatePlayerDices();
    }

    //Add resources to player
    public void AssigneResources()
    {
        if(dicesRolled)
        {
            rerolls = 0;
            if (!resourcesAssigned)
            {
                ExitLookupMode();
                AddResources();
                resourcesAssigned = true;
            }
        }
    }

    //Add active abilities to player
    public void AssigneAbilities()
    {
        foreach(string ability in player.abilitiesList)
        {
            Instantiate(Resources.Load("Abilities/" + ability + "Button"), abilitiesPanel.gameObject.transform);
        }
    }

    //Delete all resources from temporal panel
    private void ClearTemporalPanel()
    {
        foreach (Transform child in temporalResourcePanel)
        {
            Destroy(child.gameObject);
        }
    }

    //Instantiate all player's dices
    public void InstantiatePlayerDices()
    {
        if (!dicesInMove)
        {
            dicesRolled = false;
            player.InstantiateAllPlayerDices();
            FindeDices();
        }
    }

    //Instantiate dices selected to re-roll
    public void InstantiateDicesToReroll()
    {
        if (!playerTurn)
        {
            Debug.Log("Enemy turn, can't take this action");
            return;
        }
        if (dicesToReroll.Count == 0)
        {
            Debug.Log("No dies selected");
            return;
        }
        if (rerolls <= 0)
        {
            Debug.Log("No more re-rolls avalaible for thin turn");
            return;
        }
        if (!dicesInMove)
        {
            ExitLookupMode();
            dicesRolled = false;
            player.InstantiateDicesToReroll(dicesToReroll);
            FindeDices();
        }
    }

    //Throw dices from player hand to point inicated by mouse
    public void ThrowPlayerDices()
    {
        player.playerThrowDirection = GetWorldPoint();
        player.ThrowDices();
        StartCoroutine(DicesRolling());
        dicesToReroll.Clear();
        rerolls--;
        dicesInMove = true;
    }

    //Finde dices existing in world with "PlayerDice" tag
    private void FindeDices()
    {
        goDices = null;
        goDices = GameObject.FindGameObjectsWithTag("PlayerDice");
        Dices = new List<Dice>();
        foreach (GameObject go in goDices)
        {
            Dices.Add(go.GetComponent<Dice>());
        }
    }

    //Add selected dice to reroll


    public void addDiceToReroll()
    {
        Dice dice = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData))
        {
            dice = hitData.transform.gameObject.GetComponent<Dice>();
            if(dice != null && !dicesToReroll.Exists(x => x == dice.name))
            {
                dice.gameObject.layer = LayerMask.NameToLayer("Outline");
                dicesToReroll.Add(dice.name);
            }
            else if(dice != null && dicesToReroll.Exists(x => x == dice.name))
            {
                dice.gameObject.layer = LayerMask.NameToLayer("No Outline");
                dicesToReroll.Remove(dice.name);
            }
        }
    }

    //Get posisition idicated by mouse in world
    public Vector3 GetWorldPoint()
    {
        Vector3 worldPosition = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData))
        {
            worldPosition = hitData.point;
        }
        return worldPosition;
    }

    //Use send ability
    public void ExecuteAbility(Ability ability, Entity caster, Entity abilityEnemy, Entity abilityAlly)
    {
        foreach(KeyValuePair<Resource, int> token in ability.costDictionary)
        {
            if(player.resourceDictionary.TryGetValue(token.Key.type+"Tier" + token.Key.tier, out int value))
            {
                if(value < token.Value)
                {
                    Debug.Log("Not enough resource of selected type: " + token.Value + " requaired, " + value + "in stock.");
                    return;
                }
                else
                {
                    player.resourceDictionary[token.Key.type + "Tier" + token.Key.tier] -= token.Value;
                }
            }
            else
            {
                Debug.Log("No resource of selected type");
                return;
            }
        }

        caster.PlayAbilityVisuals(ability.animatioName, ability.VisualName, ability.spawnAtTarget, ability.target);
        

        foreach (Effect effect in ability.effects)
        {
            effect.Execute(abilityEnemy, abilityAlly);
        }
    }

    //Refresh values in UI
    public void UpdateUI()
    {
        playerHealth.text = player.Health.ToString();
        enemyHealth.text = enemy.Health.ToString();
        gamePhaseText.text = entityTurn + "\n" + turnPhase;

        foreach(Transform panel in resourcesMainPanel)
        {
            string type = panel.gameObject.name.Replace("Panel","");
            foreach (Transform resourcePanel in panel)
            {
                int tier = Int32.Parse(resourcePanel.gameObject.name.Replace("SingleResource", "").Replace("Tier", ""));
                int tokensToActivate;
                player.resourceDictionary.TryGetValue(type + "Tier" + tier, out tokensToActivate);
                foreach (Transform resource in resourcePanel)
                {
                    resource.gameObject.SetActive(false);
                }
                for(int i = 0; i < tokensToActivate; i++)
                {
                    foreach (Transform resource in resourcePanel)
                    {
                        if (!resource.gameObject.activeSelf)
                        {
                            resource.gameObject.SetActive(true);
                            break;
                        }
                    }
                }
            }
        }
    }

    //Main logic of turn based encounter
    IEnumerator Encounter()
    {
        rolled = false;
        onEncounter = true;
        playerTurn = true;
        InstantiatePlayerDices();

        turnPhase = TurnPhase.Upkeep;
        while (onEncounter)
        {
            statusCheck = false;
            resourcesAssigned = false;
            statusCheck = false;
            rolled = false;
            rerolls = 3;

            entityTurn = "Player Turn";
            //Player Turn
            turnPhase = TurnPhase.Upkeep;
            while (playerTurn && onEncounter)
            {
                //Player Upkeep phase
                while(turnPhase == TurnPhase.Upkeep && onEncounter)
                {
                    if(!statusCheck)
                    {
                        //Execute all player effects triggered on upkeep
                        foreach (Status status in player.statusesList)
                        {
                            if(status.triggerPhase == TurnPhase.Upkeep)
                            {
                                status.Execute();
                            }
                        }
                        statusCheck = true;
                    }
                    yield return null;
                }
                //Player Main phase
                while (turnPhase == TurnPhase.Main && onEncounter)
                {
                    yield return null;
                }

                if(turnPhase == TurnPhase.End)
                {
                    FinishTurn();
                }

                yield return null;
            }
            resourcesAssigned = false;
            rolled = false;
            statusCheck = false;
            rerolls = 3;

            entityTurn = "Enemy Turn";
            //Enemy Turn
            turnPhase = TurnPhase.Upkeep;
            while (!playerTurn && onEncounter)
            {
                //Enemy Upkeep phase
                while (turnPhase == TurnPhase.Upkeep && onEncounter)
                {
                    if (!statusCheck)
                    {
                        //Execute all enemy effects triggered on upkeep
                        foreach (Status status in enemy.statusesList)
                        {
                            if (status.triggerPhase == TurnPhase.Upkeep)
                            {
                                status.Execute();
                            }
                        }
                        statusCheck = true;
                    }
                    yield return new WaitForSeconds(2f);
                    turnPhase = turnPhase.Next(); ;
                    yield return null;
                }
                //Enemy Main phase
                while (turnPhase == TurnPhase.Main && onEncounter)
                {
                    yield return new WaitForSeconds(2f);
                    enemy.DealDamage();
                    turnPhase = turnPhase.Next();
                    playerTurn = true;
                    yield return new WaitForSeconds(2f);
                    yield return null;
                }
            }
            yield return null;
        }

        if (player.Health <= 0 && enemy.Health <= 0)
        {
            Debug.Log("Encounter finished with Draw");
            gamePhaseText.text = "Encounter finished with Draw";
        }
        else if(enemy.Health <= 0)
        {
            Debug.Log("Encounter finished, Player Won");
            gamePhaseText.text = "Encounter finished, Player Won";
            enemy.AssigneLoot();
        }
        else
        {
            Debug.Log("Encounter finished, Enemy Won");
            gamePhaseText.text = "Encounter finished, Enemy Won";
        }

        lootText.text = lootToText;
        finishPanel.gameObject.SetActive(true);

        yield break;
    }

    //Checking if dices stop rolling and assigne resources when stop
    IEnumerator DicesRolling()
    {
        // Need wait to begin any velocity 
        yield return new WaitForSeconds(1);
        do
        {
            // yield break here to collect checkZeroVelocity(), used do while to make last checkup
            yield return null;
            if (checkZeroVelocity() && !dicesRolled)
            {
                EnterLookupMode();
                AddTemporalResources();
                dicesInMove = false;
                dicesRolled = true;
                yield break;
            }
        } while (!checkZeroVelocity());
    }

    //Set camera posiiton over dices, enable lookup mode
    public void EnterLookupMode()
    {
        lookupMode = true;
        int dices = 0;
        float x = 0;
        float z = 0;
        foreach (GameObject dice in goDices)
        {
            if(dice != null)
            {
                dices++;
                x += dice.transform.position.x;
                z += dice.transform.position.z;
            }
        }

        x /= dices;
        z /= dices;

        eyeCamera.transform.parent = null;
        cameraTargetLocation = new Vector3(x, 15f, z);
        cameraTargetRotation = new Vector3(90f, 0f, 0f);
    }

    //Set camera to origin, disable lookup mode
    public void ExitLookupMode()
    {
        freeCameraMovement = false;
        eyeCamera.transform.parent = eyeGo.transform;
        cameraTargetLocation = eyeGo.transform.position;
        cameraTargetRotation = eyeGo.transform.eulerAngles;
        backToCameraOrigin = true;
    }

    //Add resources to temporal panel
    private void AddTemporalResources()
    {
        ClearTemporalPanel();

        foreach (Dice dice in Dices)
        {
            string type = "";
            int tier = 0;
            if (dice != null)
            {
                dice.setChoosedFace();
                dice.rolled = true;
                type = dice.choosedFace.type;
                tier = dice.choosedFace.tier;

                GameObject token = (GameObject)Resources.Load("UiTokens/" + type + tier);
                if (token != null)
                {
                    token = Instantiate(token, temporalResourcePanel, false);
                }
            }
        }
    }

    //Add resources to main resources panel
    private void AddResources()
    {
        foreach (Dice dice in Dices)
        {
            string type = "";
            int tier = 0;
            if (dice != null)
            {
                dice.setChoosedFace();
                dice.rolled = true;
                type = dice.choosedFace.type;
                tier = dice.choosedFace.tier;

                Transform panel = resourcesMainPanel.Find(type + "Panel");
                if (panel != null)
                {
                    int points;
                    Transform tierPanel;
                    if (player.resourceDictionary.TryGetValue(type + "Tier" + tier, out points))
                    {
                        if (points < resourceCapacity)
                        {
                            player.resourceDictionary[type + "Tier" + tier]++;
                            tierPanel = panel.Find("SingleResource" + tier + "Tier");
                            foreach (Transform child in tierPanel)
                            {
                                if (!child.gameObject.activeSelf)
                                {
                                    child.gameObject.SetActive(true);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Cannot finde proper player resource: " + type + "Tier " + tier);
                    }
                }
            }
        }
    }

    //Check if every dices stopped moving
    bool checkZeroVelocity()
    {
        foreach (Dice dice in Dices)
        {
            if (dice != null)
            {
                if (dice.rb.velocity != Vector3.zero)
                {
                    return false;
                }
            }
        }
        return true;
    }

    //Load data from save file
    public void LoadData(GameData data)
    {
        //Nothing to load now
    }

    //Save data to save file
    public void SaveData(ref GameData data)
    {
        //Nothing to save now
    }
}
