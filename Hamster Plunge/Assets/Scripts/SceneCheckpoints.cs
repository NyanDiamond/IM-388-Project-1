using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneCheckpoints : MonoBehaviour
{
    static int currentScene = 0;
    PlayerControls playerControls;
    InputAction advance;
    InputAction revert;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Default.Enable();
        advance = playerControls.FindAction("AdvanceScene");
        revert = playerControls.FindAction("revertScene");

        advance.performed += ctx => ChangeScene(1);
        revert.performed += ctx => ChangeScene(-1);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(currentScene);
    }

    private void ChangeScene(int changeDir)
    {
        currentScene += changeDir;
        // Numbers chosen arbitrarily based on the number of scenes we have
        if (currentScene < 0)
        {
            currentScene = 0;
        }
        else if (currentScene >= 4)
        {
            currentScene = 3;
        }
        SceneManager.LoadScene(currentScene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChangeScene(1);
    }

   
    private void OnEnable()
    {
        playerControls.Enable();
    }

    
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
