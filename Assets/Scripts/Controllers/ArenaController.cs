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
        MultiSceneVariables.ArenaEnemyToSpawn = "Jailer";
        SceneManager.LoadScene("Arena");
    }

    public void FightGuardian()
    {
        MultiSceneVariables.ArenaEnemyToSpawn = "Guardian";
        SceneManager.LoadScene("Arena");
    }

    public void FightCaveLord()
    {
        MultiSceneVariables.ArenaEnemyToSpawn = "CaveLord";
        SceneManager.LoadScene("Arena");
    }

    public void BackToHub()
    {
        SceneManager.LoadScene("HubMenu");
    }
}
