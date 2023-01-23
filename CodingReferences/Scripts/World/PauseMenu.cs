using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to contol the pause menu and all the aspects used by the buttons
*/
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button firstButton;
    // Start is called before the first frame update

    public void OnPause()
    {
        if (!pauseMenu.activeInHierarchy && Time.timeScale == 1)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            firstButton.Select();
        }
        else if (pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    public void OnQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); 
    }

    public void OnRestart()
    {
        Time.timeScale = 1f;
        Save.LoadCheckpoint();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnResume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
