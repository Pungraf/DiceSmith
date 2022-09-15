using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameController : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private bool dicesRolled;
    [SerializeField]
    private bool dicesInMove;
    [SerializeField]
    private int resourceCapacity;
    [SerializeField]
    private TMP_Text playerHealth;
    [SerializeField]
    private TMP_Text enemyrHealth;

    // TODO fix empty objects in goDices and Dices ( now fixed by null sheckup in methods )
    private GameObject[] goDices;
    private List<string> dicesToReroll;
    private List<Dice> Dices = new List<Dice>();
    private Player player;
    private Vector3 throwDirection;
    private Coroutine rollCoroutine;

    [SerializeField]
    private RectTransform resourcesMainPanel;
    [SerializeField]
    private GameObject resourcesPanelPrefab;

    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        dicesToReroll = new List<string>();
        dicesRolled = false;
        dicesInMove = false;
        FindeDices();

        foreach(string magicType in player.magicTypes)
        {
            GameObject panel =  Instantiate(resourcesPanelPrefab);
            panel.transform.SetParent(resourcesMainPanel, false);
            panel.name = magicType + "Panel";
        }

        player.Health = 30;
        playerHealth.text = player.Health.ToString();
        enemyrHealth.text = 30.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetWorldPoint() != Vector3.zero && !dicesInMove && !dicesRolled)
        {
            ThrowPlayerDices();
        }

        if (Input.GetMouseButtonDown(1) && GetWorldPoint() != Vector3.zero && !dicesInMove && dicesRolled)
        {
            addDiceToReroll();
        }

        /*if (Input.GetKeyDown(KeyCode.P) && !dicesInMove)
        {
            PrototypeDeleteToken();
        }*/
    }

    /*private void PrototypeAddToken()
    {
        Instantiate(firstTierToken).transform.SetParent(panel, false);
    }

    private void PrototypeDeleteToken()
    {
        int numChildren = panel.transform.childCount;
        Destroy(panel.transform.GetChild(numChildren - 1).gameObject);
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

    // Testing method
    public void AttackPlayer(int damage)
    {
        player.Health -= damage;
        playerHealth.text = player.Health.ToString();
    }

    // Testing method
    public void AttackEnemy(int damage)
    {
        int enemyHP = Int32.Parse(enemyrHealth.text);
        enemyHP -= damage;
        enemyrHealth.text = enemyHP.ToString();
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
                            GameObject token;
                            Transform tierPanel;
                            switch (tier)
                            {
                                case 1:
                                    if(player.resourceDictionary.TryGetValue(type + "TierOne", out points))
                                    {
                                        if(points < resourceCapacity)
                                        {
                                            player.resourceDictionary[type + "TierOne"]++;
                                            tierPanel = panel.Find("SingleResourceFirstTier");
                                            token = (GameObject)Instantiate(Resources.Load("UiTokens/" + type + tier));
                                            token.transform.SetParent(tierPanel, false);
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("Cannot finde proper player resource: " + type + "TierOne");
                                    }
                                    break;
                                case 2:
                                    if (player.resourceDictionary.TryGetValue(type + "TierTwo", out points))
                                    {
                                        if (points < resourceCapacity)
                                        {
                                            player.resourceDictionary[type + "TierTwo"]++;
                                            tierPanel = panel.Find("SingleResourceSecondTier");
                                            token = (GameObject)Instantiate(Resources.Load("UiTokens/" + type + tier));
                                            token.transform.SetParent(tierPanel, false);
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("Cannot finde proper player resource: " + type + "TierTwo");
                                    }
                                    break;
                                case 3:
                                    if (player.resourceDictionary.TryGetValue(type + "TierThree", out points))
                                    {
                                        if (points < resourceCapacity)
                                        {
                                            player.resourceDictionary[type + "TierThree"]++;
                                            tierPanel = panel.Find("SingleResourceThirdTier");
                                            token = (GameObject)Instantiate(Resources.Load("UiTokens/" + type + tier));
                                            token.transform.SetParent(tierPanel, false);
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("Cannot finde proper player resource: " + type + "TierThree");
                                    }
                                    break;
                            }
                        }
                    }
                }
                dicesInMove = false;
                dicesRolled = true;
                yield break;
            }
        } while (!checkZeroVelocity());
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
