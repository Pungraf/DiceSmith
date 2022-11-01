using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaController : MonoBehaviour
{
    public void FightJailor()
    {
        SceneManager.LoadScene("JailerRoom");
    }

    public void BackToHub()
    {
        SceneManager.LoadScene(1);
    }
}
