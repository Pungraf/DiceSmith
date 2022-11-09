using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : Ability
{
    [SerializeField]
    private Transform costPanel;

    public bool isPersistance;

    public override void Start()
    {
        base.Start();
        costPanel = this.transform.Find("Cost");
    }

    public void AssigneCost()
    {
        if (isPersistance)
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
}
