using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoneSpear : Ability
{
    [SerializeField]
    private TMP_Text damageValue;
    [SerializeField]
    private int damage;

    public new void Start()
    {
        base.Start();
        effects.Add(new DealDamageEffect(damage));
        damageValue.text = damage.ToString();
    }
}
