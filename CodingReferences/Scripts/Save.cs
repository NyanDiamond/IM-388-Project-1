using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ian Connors 
 * Project: F.E.I.N.T
 * This code is used to save and load checkpoints when necessary
*/
public class Save : MonoBehaviour
{
    // Start is called before the first frame update
    public static void SaveCheckpoint(int value)
	{
        PlayerPrefs.SetInt("checkpoint", value);
        //saves a bool so that when the game loads the next scene it can display the checkpoint notice on start
        //this only takes effect when a scene is loaded without using the LoadCheckpoint method
        PlayerPrefs.SetInt("NewCheckpoint", 1);
	}

    // Update is called once per frame
    public static void LoadCheckpoint()
    {
        int temp;
        temp = PlayerPrefs.GetInt("checkpoint");
        Debug.Log("Going to Checkpoint " + temp);
        PlayerPrefs.SetInt("NewCheckpoint", 0);
        CheckpointLoader.Load(temp);
    }
    public static void LoadFirstCheckpoint()
	{
        CheckpointLoader.LoadCutscene(0);
        PlayerPrefs.SetInt("NewCheckpoint", 0);
    }
}
