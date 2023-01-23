using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ian Connors 
 * Project: F.E.I.N.T
 * This code is used to contol the camera when entering a new room
*/
public class RoomEntry : MonoBehaviour
{
    private GameObject mainCamera;
    public GameObject door;
    public bool isDark;
    private bool isTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");   
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            EnemyCounter.doorsave.Add(door);
            Debug.Log("Added door");
            if (isDark)
                EnemyCounter.EnterRoom(true);
            else
            {
                EnemyCounter.EnterRoom(false);
            }
        }
	}
}
