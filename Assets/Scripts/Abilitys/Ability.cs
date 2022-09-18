using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Ability : MonoBehaviour
{
    public List<Effect> effects = new List<Effect>();

    private GameController gameController;

    public void Start()
    {
        gameController = (GameController)FindObjectOfType(typeof(GameController));
        Debug.Log("Alive");
    }

    public void Cast()
    {
        gameController.ExecuteAbility(this);
    }
}
