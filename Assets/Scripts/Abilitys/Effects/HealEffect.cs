using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : Effect
{
    public HealEffect(int amount)
    {
        _amount = amount;
    }

    private int _amount;

    public override void Execute(Entity entity, Entity ally)
    {
        ally.Health += _amount;
    }
}
