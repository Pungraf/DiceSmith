using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Resource
{
    public Resource(string _type, int _tier)
    {
        type = _type;
        tier = _tier;
    }

    public string type;
    public int tier;
}
