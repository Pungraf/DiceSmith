using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour, IDataPersistence
{
    private bool dicesRolled;
    private bool dicesInMove;
    // TODO fix empty objects in goDices and Dices ( now fixed by null sheckup in methods )
    private GameObject[] goDices;
    [SerializeField]
    private List<string> dicesToReroll;
    private List<Dice> Dices = new List<Dice>();
    private Player player;
    private Vector3 throwDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        dicesRolled = false;
        dicesInMove = false;
        FindeDices();
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

        if (dicesInMove && !dicesRolled)
        {
            StartCoroutine(DicesRolling());
        }
    }

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

    IEnumerator DicesRolling()
    {
        yield return new WaitForSeconds(1);
        if (checkZeroVelocity() && !dicesRolled)
        {
            foreach (Dice dice in Dices)
            {
                if (dice != null)
                {
                    dice.setChoosedFace();
                    dice.rolled = true;
                }
            }

            dicesInMove = false;
            dicesRolled = true;
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
