using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public string abilityName;
    public bool abilitySelected = false;

    private PlayerAbility ability;
    private GameObject abilitySheetGo;
    private RectTransform abilitySheet;
    private RectTransform abilityPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Configuration()
    {
        abilitySheetGo = (GameObject) Instantiate(Resources.Load("AbilitiesSheets/" + abilityName), transform);
        ability = abilitySheetGo.GetComponent<PlayerAbility>();
        ability.isPersistance = true;
        abilitySheet = abilitySheetGo.GetComponent<RectTransform>();
        abilityPanel = (RectTransform)abilitySheet.GetChild(0);
        this.GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!abilitySelected)
            {
                abilitySelected = true;
                AbilityController.Instance.AddPlayerAbility(abilityName);
                abilityPanel.GetComponent<Image>().color = Color.cyan;

                Debug.Log("Adding");
            }
            else
            {
                abilitySelected = false;
                AbilityController.Instance.RemovePlayerAbility(abilityName);
                abilityPanel.GetComponent<Image>().color = Color.white;

                Debug.Log("Removing");
            }
        };
        if(abilitySelected)
        {
            abilityPanel.GetComponent<Image>().color = Color.cyan;
        }
        else
        {
            abilityPanel.GetComponent<Image>().color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
