using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Ian Connors 
 * Project: F.E.I.N.T
 * This code is used to save the current checkpoint
*/
public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    public int checkpointNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            /*environment.transform.GetChild(nextRoom - 1).gameObject.SetActive(true);
            EnemyCounter.enemies.Clear();
            EnemyCounter.count = 0;
            collision.gameObject.transform.SetPositionAndRotation(nextRoomEntryZone.transform.position, Quaternion.identity);
            environment.transform.GetChild(currentRoom - 1).gameObject.SetActive(false);*/
            Debug.Log("Saving checkpoint " + checkpointNumber);
            Save.SaveCheckpoint(checkpointNumber);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyInfoController>().CheckpointNotice(checkpointNumber);
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
