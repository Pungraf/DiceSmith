using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianHitVisual : VisualEffect
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
