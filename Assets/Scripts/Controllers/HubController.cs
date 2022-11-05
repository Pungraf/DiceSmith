using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubController : MonoBehaviour
{
    public void Fight()
    {
        SceneManager.LoadScene("ArenaMenu" );
    }

    public void Abilities()
    {
        SceneManager.LoadScene("Abilities");
    }

    public void DiceSmith()
    {
        SceneManager.LoadScene("Smith");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
