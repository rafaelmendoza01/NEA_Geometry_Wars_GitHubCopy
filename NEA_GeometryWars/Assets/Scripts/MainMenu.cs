using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //used to change scenes in Unity

public class MainMenu : MonoBehaviour
{
    //To load up the game
    public void PlayGame()
    {
        PlayerMovement.KillsForLevel = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
