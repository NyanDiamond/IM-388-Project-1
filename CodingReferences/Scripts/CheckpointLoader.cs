using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Ian Connors 
 * Project: F.E.I.N.T
 * This code is used load certain levels based on the current checkpoint or cutscene
*/

public class CheckpointLoader : MonoBehaviour
{


    public static void Load(int checkpoint)
	{
		if (checkpoint == 0)
		{
			SceneManager.LoadScene("Tutorial");
			Save.SaveCheckpoint(0);
		}
		else if (checkpoint == 1)
		{
			SceneManager.LoadScene("Level 1");
		}
		else if (checkpoint == 2)
		{
			SceneManager.LoadScene("Level 1B");
		}
		else if (checkpoint == 3)
		{
			SceneManager.LoadScene("Level 1C");
		}
		else if (checkpoint == 4)
		{
			SceneManager.LoadScene("Level 2");
		}
		else if (checkpoint == 5)
		{
			SceneManager.LoadScene("Level 2B");
		}
		else if (checkpoint == 6)
		{
			SceneManager.LoadScene("Level 3");
		}

	}
	public static void LoadCutscene(int number)
	{
		if (number == 0)
		{
			SceneManager.LoadScene("Cutscene1");
		}
		else if (number == 1)
		{
			SceneManager.LoadScene("Cutscene2");
		}
		else if (number == 2)
		{
			SceneManager.LoadScene("Cutscene3");
		}
	}
}
