using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffects : MonoBehaviour
{
    [SerializeField]
    private Transform hand;
    [SerializeField]
    private Transform enemy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnVisuals(string visualsName, Transform target = null)
    {
        GameObject abilityVisual;
        if (target == null)
        {
            abilityVisual = (GameObject)Instantiate(Resources.Load("AbilityVisuals/" + visualsName), hand.position, Quaternion.Euler(90f, 0f, 0f));
        }
        else
        {
            abilityVisual = (GameObject)Instantiate(Resources.Load("AbilityVisuals/" + visualsName), hand.position, Quaternion.Euler(90f, 0f, 0f));
            abilityVisual.GetComponent<VisualEffect>().targetLocation = target;
        }
    }
}
