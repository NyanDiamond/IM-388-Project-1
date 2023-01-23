using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSceneController : MonoBehaviour
{
    [SerializeField] PauseMenu winMenu;
    // Start is called before the first frame update
    void Start()
    {
        winMenu.OnPause();
        Time.timeScale = 1f;
    }
}
