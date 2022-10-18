using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSpearVisual : VisualEffect
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetLocation.position, Time.deltaTime * 1);

        if ((Vector3.Distance(transform.position, targetLocation.position) <= 1f))
        {
            Destroy(this.gameObject);
        }
    }
}
