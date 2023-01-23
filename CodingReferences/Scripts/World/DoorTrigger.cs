using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Ian Connors
 * Project: F.E.I.N.T
 * This code controls the rooms moving in and out of existance when activated
*/
public class DoorTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextRoomEntryZone;
    public GameObject environment;
    public int currentRoom;
    public int nextRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            environment.transform.GetChild(nextRoom - 1).gameObject.SetActive(true);
            EnemyCounter.enemies.Clear();
            EnemyCounter.count = 0;
            collision.gameObject.transform.SetPositionAndRotation(nextRoomEntryZone.transform.position, Quaternion.identity);
            environment.transform.GetChild(currentRoom - 1).gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthBehavior>().RecoverHealth();
        }
    }
	private void OnDisable()
	{
        EnemyCounter.doorsave.Remove(gameObject);
	}
}
