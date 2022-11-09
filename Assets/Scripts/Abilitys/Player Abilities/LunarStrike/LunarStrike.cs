using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LunarStrike : PlayerAbility
{
    [SerializeField]
    private TMP_Text damageValue;
    [SerializeField]
    private int damage;

    public override void Start()
    {

        costDictionary.Add(new Resource("Night", 2), 2);
        costDictionary.Add(new Resource("Night", 3), 1);
        base.Start();
        effects.Add(new DealDamageEffect(damage));
        damageValue.text = damage.ToString();
    }
}
