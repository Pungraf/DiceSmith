using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barier : EnemyAbility
{
    public int _value;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        effects.Add(new BarierEfect(_value));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
