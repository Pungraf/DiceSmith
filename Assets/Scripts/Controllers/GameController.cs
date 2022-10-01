using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameController : MonoBehaviour, IDataPersistence
{
    public int rerolls;
    public bool endTurn;

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

    // TODO fix empty objects in goDices and Dices ( now fixed by null checkup in methods )
    private GameObject[] goDices;
    private List<string> dicesToReroll;
    private List<Dice> Dices = new List<Dice>();
    private Player player;
    private Enemy enemy;
    private Vector3 throwDirection;
    private Coroutine rollCoroutine;
    private bool onEncounter;
    private bool playerTurn;
    private bool rolled;
    private bool resourcesAssigned;

    [SerializeField]
    private RectTransform resourcesMainPanel;
    [SerializeField]
    private RectTransform temporalResourcePanel;
    [SerializeField]
    private GameObject resourcesPanelPrefab;

    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        dicesToReroll = new List<string>();
        dicesRolled = false;
        dicesInMove = false;
        FindeDices();

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


        player.Health = 30;
        enemy.Health = 30;
        playerHealth.text = player.Health.ToString();
        enemyHealth.text = enemy.Health.ToString();


        StartCoroutine(Encounter());
    }

    // Update is called once per frame
    void Update()
    {
        if(onEncounter && playerTurn)
        {
            if (Input.GetMouseButtonDown(0) && GetWorldPoint() != Vector3.zero && !dicesInMove && !dicesRolled)
            {
                ThrowPlayerDices();
            }

            if (Input.GetMouseButtonDown(1) && GetWorldPoint() != Vector3.zero && !dicesInMove && dicesRolled)
            {
                addDiceToReroll();
            }
        }

        if(player.Health <= 0 || enemy.Health <= 0)
        {
            onEncounter = false;
        }
    }

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
        ClearTemporalPanel();
        playerTurn = false;

        InstantiatePlayerDices();
    }

    public void AssigneResources()
    {
        if(dicesRolled)
        {
            rerolls = 0;
            if (!resourcesAssigned)
            {
                AddResources();
                resourcesAssigned = true;
            }
        }
    }

    private void ClearTemporalPanel()
    {
        foreach (Transform child in temporalResourcePanel)
        {
            Destroy(child.gameObject);
        }
    }


    /*public void InstantiatePlayerDices()
    {
        if(!playerTurn)
        {
            Debug.Log("Enemy turn, can't take this action");
            return;
        }
        if(rerolls <= 0 || rolled)
        {
            Debug.Log("No more rolls avalaible for thin turn");
            return;
        }
        rolled = true;
        if (!dicesInMove)
        {
            dicesRolled = false;
            player.InstantiateAllPlayerDices();
            FindeDices();
        }
    }*/

    public void InstantiatePlayerDices()
    {
        if (!dicesInMove)
        {
            dicesRolled = false;
            player.InstantiateAllPlayerDices();
            FindeDices();
        }
    }

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
            dicesRolled = false;
            player.InstantiateDicesToReroll(dicesToReroll);
            FindeDices();
        }
    }

    public void ThrowPlayerDices()
    {
        player.ThrowDices(GetWorldPoint());
        StartCoroutine(DicesRolling());
        dicesToReroll.Clear();
        rerolls--;
        dicesInMove = true;
    }

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



    public void addDiceToReroll()
    {
        Dice dice = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData))
        {
            dice = hitData.transform.gameObject.GetComponent<Dice>();
            if(dice != null && !dicesToReroll.Exists(x => x == dice.name))
            {
                dicesToReroll.Add(dice.name);
            }
        }
    }

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

    public void ExecuteAbility(Ability ability)
    {
        foreach(KeyValuePair<Resource, int> token in ability.costDictionary)
        {
            if(player.resourceDictionary.TryGetValue(token.Key.type+"Tier" + token.Key.tier, out int value))
            {
                if(value < token.Value)
                {
                    Debug.Log("Not enough resource of selected type");
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
        foreach(Effect effect in ability.effects)
        {
            effect.Execute(enemy, player);
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        playerHealth.text = player.Health.ToString();
        enemyHealth.text = enemy.Health.ToString();

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

    IEnumerator Encounter()
    {
        rolled = false;
        onEncounter = true;
        playerTurn = true;
        InstantiatePlayerDices();
        while (onEncounter)
        {
            resourcesAssigned = false;
            rolled = false;
            rerolls = 3;

            Debug.Log("Player Turn");
            while (playerTurn)
            {
                if(endTurn)
                {
                    playerTurn = false;
                }
                yield return null;
            }
            resourcesAssigned = false;
            rolled = false;
            rerolls = 3;

            Debug.Log("Enemy Turn");
            while (!playerTurn)
            {
                yield return new WaitForSeconds(1);
                Debug.Log("Enemy dealed damage");
                enemy.DealDamage();
                UpdateUI();
                yield return new WaitForSeconds(2);
                playerTurn = true;
                yield return null;
            }
            yield return null;
        }
        if(player.Health <= 0 && enemy.Health <= 0)
        {
            Debug.Log("Encounter finished with Draw");
        }
        else if(enemy.Health <= 0)
        {
            Debug.Log("Encounter finished, Player Won");
        }
        else
        {
            Debug.Log("Encounter finished, Enemy Won");
        }

        yield break;
    }

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
                AddTemporalResources();
                //AddResources();
                dicesInMove = false;
                dicesRolled = true;
                yield break;
            }
        } while (!checkZeroVelocity());
    }

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

    public void LoadData(GameData data)
    {
        //Nothing to load now
    }

    public void SaveData(ref GameData data)
    {
        //Nothing to save now
    }
}
