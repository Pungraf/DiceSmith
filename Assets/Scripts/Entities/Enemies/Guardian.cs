using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Health = 20;
        statusesList.Add(new Status());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(statusesList.Count);
    }
}
