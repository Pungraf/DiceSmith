using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    public DealDamageEffect(int damage = 0)
    {
        _damage = damage;
    }

    public int _damage;

    public override void Execute(Entity entity, Entity ally)
    {
        entity.GetDamage(_damage);
    }

}
