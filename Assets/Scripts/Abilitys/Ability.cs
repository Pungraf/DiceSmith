using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Ability : MonoBehaviour
{
    public string animatioName;
    public string VisualName;
    public bool target;
    public bool spawnAtTarget;
    public List<Effect> effects;
    public Dictionary<Resource, int> costDictionary;


    public virtual void Start()
    {
        effects = new List<Effect>();
        costDictionary = new Dictionary<Resource, int>();
        effects.Clear();

    }

    public void Cast(Entity caster, Entity abilityEnemy, Entity abilityAlly)
    {
        GameController.Instance.ExecuteAbility(this, caster, abilityEnemy, abilityAlly);
    }
}
