using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            SceneManager.LoadScene(0);
        }
    }
}
