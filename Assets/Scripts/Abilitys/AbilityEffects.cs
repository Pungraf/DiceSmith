using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffects : MonoBehaviour
{
    [SerializeField]
    private Transform enemy;
    public Transform hand;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnVisuals(string visualsName, Transform spawn, Transform target)
    {
        GameObject abilityVisual;
        if (target == null)
        {
            abilityVisual = (GameObject)Instantiate(Resources.Load("AbilityVisuals/" + visualsName), spawn.position, Quaternion.Euler(90f, 0f, 0f));
            abilityVisual.GetComponent<VisualEffect>().spawnLocation = spawn;
        }
        else
        {
            abilityVisual = (GameObject)Instantiate(Resources.Load("AbilityVisuals/" + visualsName), spawn.position, Quaternion.Euler(90f, 0f, 0f));
            abilityVisual.GetComponent<VisualEffect>().targetLocation = target;
            abilityVisual.GetComponent<VisualEffect>().spawnLocation = spawn;
        }
    }
}
