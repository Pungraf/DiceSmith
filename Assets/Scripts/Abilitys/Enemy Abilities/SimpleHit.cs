using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHit : EnemyAbility
{
    public int _damage;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        effects.Add(new DealDamageEffect(_damage));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
