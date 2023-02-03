using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    SceneCheckpoints sceneCheckpoints;

    private void Awake()
    {
        sceneCheckpoints = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneCheckpoints>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Quit!");
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("Restart!");
            sceneCheckpoints.ResetScene();
        }
    }
}
