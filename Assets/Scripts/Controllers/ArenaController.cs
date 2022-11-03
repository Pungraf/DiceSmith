using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ArenaController : MonoBehaviour
{
    public void FightJailor()
    {
        SceneManager.LoadScene("JailerRoom");
    }

    public void FightGuardian()
    {
        SceneManager.LoadScene("GuardianRoom");
    }

    public void FightCaveLord()
    {
        SceneManager.LoadScene("CaveLordRoom");
    }

    public void BackToHub()
    {
        SceneManager.LoadScene(1);
    }
}
