using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Ability : MonoBehaviour
{
    private int damage;
    private GameController gameController;

    [SerializeField]
    private TMP_Text damageValue;

    public void Start()
    {
        gameController = (GameController)FindObjectOfType(typeof(GameController));
        damage = Int32.Parse(damageValue.text);
    }

    public void Cast()
    {
        gameController.AttackEnemy(damage);
    }
}
