using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStatus : Status, IShieldEffect
{

    public int _shieldValue;

    public ShieldStatus(int shieldValue)
    {
       _shieldValue = shieldValue;
    }


    public int UseShield(int damage)
    {
        _shieldValue -= damage;
        return _shieldValue;
    }
}
