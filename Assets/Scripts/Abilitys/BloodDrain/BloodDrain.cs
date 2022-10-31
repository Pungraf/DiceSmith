using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BloodDrain : Ability
{
    [SerializeField]
    private TMP_Text damageValue;
    [SerializeField]
    private int damage;

    [SerializeField]
    private TMP_Text healValue;
    [SerializeField]
    private int heal;

    public override void Start()
    {

        costDictionary.Add(new Resource("Blood", 1), 1);
        costDictionary.Add(new Resource("Blood", 2), 1);

        base.Start();
        effects.Add(new DealDamageEffect(damage));
        damageValue.text = damage.ToString();
        effects.Add(new HealEffect(heal));
        healValue.text = heal.ToString();
    }
}
