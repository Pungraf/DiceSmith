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

    public Ability ability;
    // Start is called before the first frame update
    public void Start()
    {
        abilitySheet.gameObject.SetActive(false);
        abilitySheet.GetComponent<Ability>().isPersistance = true;
        ability = abilityGo.GetComponent<Ability>();
        ability.Start();
        this.GetComponent<Button_UI>().MouseOverOnceFunc = () =>
        {
            abilitySheet.gameObject.SetActive(true);
            abilitySheet.GetComponent<Ability>().AssigneCost();
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
        ability.Cast();
    }
}
