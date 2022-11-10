using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoneShield : PlayerAbility
{
    [SerializeField]
    private TMP_Text barierValue;
    [SerializeField]
    private int barier;

    public override void Start()
    {
        base.Start();

        costDictionary.Add(new Resource("Bone", 2), 2);

        effects.Add(new BoneShieldEffect(barier));
        barierValue.text = barier.ToString();
        AssigneCost();
    }
}
