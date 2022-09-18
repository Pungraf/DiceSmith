using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    public DealDamageEffect(int damage)
    {
        _damage = damage;
    }

    private int _damage;

    public override void Execute(Entity entity, Entity ally)
    {
        entity.Health -= _damage;
    }

}
