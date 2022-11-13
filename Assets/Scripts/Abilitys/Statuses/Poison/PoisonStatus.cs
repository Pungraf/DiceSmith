using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStatus : Status
{
    public int _damage;

    public PoisonStatus(int value)
    {
        _damage = value;
        triggerPhase = TurnPhase.Upkeep;
    }

    public override void Execute()
    {
        base.Execute();
        host.GetDamage(_damage);
    }
}
