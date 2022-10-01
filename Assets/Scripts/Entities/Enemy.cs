using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage()
    {
        target.GetComponent<Entity>().Health -= 1;
    }
}
