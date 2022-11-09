using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [SerializeField]
    private GameObject abilityGo;
    [SerializeField]
    private GameObject abilitySheet;

    public PlayerAbility ability;
    // Start is called before the first frame update
    public void Start()
    {
        abilitySheet.GetComponent<PlayerAbility>().isPersistance = true;
        abilitySheet.GetComponent<PlayerAbility>().Start();
        abilitySheet.gameObject.SetActive(false);
        ability = abilityGo.GetComponent<PlayerAbility>();
        ability.Start();
        this.GetComponent<Button_UI>().MouseOverOnceFunc = () =>
        {
            abilitySheet.gameObject.SetActive(true);
            abilitySheet.GetComponent<PlayerAbility>().AssigneCost();
        };
        this.GetComponent<Button_UI>().MouseOutOnceFunc = () =>
        {
            abilitySheet.gameObject.SetActive(false);
        };
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CastAbility()
    {
        ability.Cast(GameController.Instance.player, GameController.Instance.player.GetEnemy(), GameController.Instance.player);
    }
}
