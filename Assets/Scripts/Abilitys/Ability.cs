using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Ability : MonoBehaviour
{
    public string animatioName;
    public string VisualName;
    public bool target;
    public Dictionary<Resource, int> costDictionary = new Dictionary<Resource, int>();
    public List<Effect> effects = new List<Effect>();
    [SerializeField]
    private Transform costPanel;

    private GameController gameController;
    public bool isPersistance;

    public virtual void Start()
    {
        effects.Clear();
        costPanel = this.transform.Find("Cost");
        gameController = (GameController)FindObjectOfType(typeof(GameController));
        AssigneCost();
    }

    public void AssigneCost()
    {
        if(isPersistance)
        {
            foreach (Transform child in costPanel)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (KeyValuePair<Resource, int> entry in costDictionary)
            {
                for (int i = 0; i < entry.Value; i++)
                {
                    GameObject resource = (GameObject)Instantiate(Resources.Load("UiTokens/" + entry.Key.type + entry.Key.tier), costPanel);
                }
            }
        }
    }

    public void Cast()
    {
        gameController.ExecuteAbility(this);
    }
}
