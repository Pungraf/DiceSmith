using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour, IDataPersistence
{
    bool dicesRolled;
    GameObject[] goDices;
    public List<Dice> Dices = new List<Dice>();

    private Player player;
    private Vector3 throwDirection;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();


        dicesRolled = false;
    //    FindeDices();
    }

    public void InstantiatePlayerDices()
    {
        player.InstantiateAllPlayerDices();
    }

    public void ThrowPlayerDices()
    {
        player.ThrowDices(GetWorldPoint());
    }

    private void FindeDices()
    {
        goDices = null;
        goDices = GameObject.FindGameObjectsWithTag("PlayerDice");
        Dices.Clear();
        foreach (GameObject go in goDices)
        {
            Dices.Add(go.GetComponent<Dice>());
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        //FindeDices();
        /*StartCoroutine(DiceDelay());

        if (checkZeroVelocity() && !dicesRolled)
        {
            foreach(Dice dice in Dices)
            {
                dice.setChoosedFace();
                Debug.Log("Choosed number: " + dice.choosedFace.FaceIndex + ", type: " + dice.choosedFace.FaceName); ;
            }

            dicesRolled = true;
        }*/

        if(Input.GetMouseButtonDown(0) && GetWorldPoint() != Vector3.zero)
        {
            ThrowPlayerDices();
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

    IEnumerator DiceDelay()
    {
        yield return new WaitForSeconds(1f);
    }

    bool checkZeroVelocity()
    {
        foreach(Dice dice in Dices)
        {
            if(dice.rb.velocity != Vector3.zero)
            {
                return false;
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
