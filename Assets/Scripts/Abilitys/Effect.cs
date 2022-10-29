using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public abstract void Execute(Entity enemy, Entity ally);
}
