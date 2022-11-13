using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CorruptedBlood : PlayerAbility
{
    [SerializeField]
    private TMP_Text damageValue;
    [SerializeField]
    private int damage;

    public override void Start()
    {
        base.Start();

        costDictionary.Add(new Resource("Blood", 3), 2);

        effects.Add(new PoisonEffect(1));
        damageValue.text = damage.ToString();

        AssigneCost();
    }
}
