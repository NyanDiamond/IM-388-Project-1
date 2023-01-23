using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to keep track of all active enemies in a room and also calls them all when it is time to break out of stealth or to tell the doors that all the enemies have been destroyed
*/
public class EnemyCounter : MonoBehaviour
{
    public static int count=0;
    public static List<GameObject> enemies = new List<GameObject>();
    public static List<GameObject> cameras = new List<GameObject>();
    public static bool countChanged = false;
    public static List<GameObject> doorsave = new List<GameObject>();
    public static List<GameObject> turrets = new List<GameObject>();
    public static EnemyInfoController enemyInfoScript;
    public static bool stealth = true;
    public static bool doorClosed = false;


    protected virtual void Awake()
    {
        enemies = new List<GameObject>();
        count = 0;
        doorsave = new List<GameObject>();
        cameras = new List<GameObject>();
        countChanged = false;
        stealth = true;
        doorClosed = false;
        //doorsave = GameObject.FindGameObjectsWithTag("Door");
        enemyInfoScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyInfoController>();
    }
    protected virtual void LateUpdate()
    {
        if (count <= 0 && countChanged && !stealth)
        {
            //Debug.Log("All enemies dead");
            countChanged = false;
            foreach (GameObject temp in doorsave)
            { 
                temp.SetActive(false);
            }
            doorClosed = false;
            enemyInfoScript.AllEnemiesDead();
            SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
            sp.PlaySound2();
        }
        else if (countChanged)
        {
            Debug.Log("Enemy count: " + count);
            countChanged = false;
            /*foreach (GameObject temp in doorsave)
            {
                temp.SetActive(true);
            }*/
        }
      
    }
    public static void upCount()
    {
        count++;
        countChanged = true;
    }

    public static void downCount()
    {
        count--;
        countChanged = true;
    }

    public static void BeginStealth()
	{
        foreach (GameObject temp in doorsave)
        {
            temp.SetActive(false);
        }
        enemyInfoScript.StealthStart();
        stealth = true;
        doorClosed = false;
    }
    public static void EnterRoom(bool isDark)
	{
        foreach (GameObject temp in doorsave)
        {
            temp.SetActive(false);
        }
        if (isDark)
		{
            enemyInfoScript.EnterDarkRoom();
		}
		else
		{
            enemyInfoScript.EnterBrightRoom();
		}
        stealth = true;
        doorClosed = false;
    }
    public static bool TurretsActive()
	{
        foreach (GameObject turret in turrets)
        {
            if (turret.GetComponent<TurretBehavior>().IsActive())
			{
                return true;
			}
        }
        return false;
    }
    public static void StealthBreak()
    {
        enemyInfoScript.StealthBreak();
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<ExperimentalEnemyMovement>().StealthBreak();
        }
        foreach(GameObject camera in cameras)
        {
            camera.GetComponent<CameraBehavior>().StealthBreak();
        }

        foreach (GameObject temp in doorsave)
        {
            if(enemies.Count>0)
                temp.SetActive(true);
        }

        foreach(GameObject turret in turrets)
        {
            Debug.Log("activate turrets");
            turret.GetComponent<TurretBehavior>().Activate();
        }
        if (enemies.Count > 0)
        {
            stealth = false;
            doorClosed = true;
        }
    }

    public static void TeleporterCheck()
    {
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponentInChildren<ExperimentalEnemyMovement>().TeleporterCheck();
        }
    }
}
