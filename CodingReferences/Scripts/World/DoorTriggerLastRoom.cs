using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * Ian Connors
 * Project: F.E.I.N.T
 * This code is used to move you to the next scene when activated and saves a checkpoint
*/
public class DoorTriggerLastRoom : MonoBehaviour
{
    // Start is called before the first frame update
    public string nextLevel;
    public int checkpointNumber;
    public bool isFinalLevel;
    public GameObject winMenu;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            /*environment.transform.GetChild(nextRoom - 1).gameObject.SetActive(true);
            EnemyCounter.enemies.Clear();
            EnemyCounter.count = 0;
            collision.gameObject.transform.SetPositionAndRotation(nextRoomEntryZone.transform.position, Quaternion.identity);
            environment.transform.GetChild(currentRoom - 1).gameObject.SetActive(false);*/
            if (isFinalLevel)
			{
                winMenu.GetComponent<PauseMenu>().OnPause();
			}
			else
			{
                Save.SaveCheckpoint(checkpointNumber);
                SceneManager.LoadScene(nextLevel);
            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthBehavior>().RecoverHealth();
        }
    }
	private void OnDisable()
	{
        EnemyCounter.doorsave.Remove(gameObject);
	}
}
