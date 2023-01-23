using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is found at the beginning of every room and is used to contain all of the relevant enemy data for the room
 */

public class EnemyContainer : MonoBehaviour
{

    [SerializeField] GameObject[] enemiesInLevel;
    [SerializeField] GameObject[] camerasInLevel;
    [SerializeField] GameObject[] turretsInLevel;
    private bool isTriggered = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            GameObject teleporter = GameObject.FindGameObjectWithTag("Teleporter");
            if(teleporter!=null)
            {
                Destroy(teleporter);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().EnterRoom();
            }

            EnemyCounter.enemies.Clear();
            EnemyCounter.count = 0;
            if(enemiesInLevel.Length>0)
            foreach (GameObject i in enemiesInLevel)
            {
                EnemyCounter.upCount();
                EnemyCounter.enemies.Add(i);
            }
            EnemyCounter.cameras.Clear();
            if(camerasInLevel.Length>0)
            foreach(GameObject i in camerasInLevel)
            {
                EnemyCounter.cameras.Add(i);
            }
            EnemyCounter.turrets.Clear();

			if (turretsInLevel.Length > 0)
            foreach (GameObject i in turretsInLevel)
            {
                EnemyCounter.turrets.Add(i);
            }
            
            DestroyAfterSeconds(0.5f);
        }
    }
    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }

}
