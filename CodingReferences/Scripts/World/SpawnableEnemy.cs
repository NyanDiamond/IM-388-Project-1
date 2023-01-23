using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This is a scriptable object that holds the data for enemies to be spawned in the final boss arena
*/
[CreateAssetMenu(fileName = "Enemy", menuName = "Spawnable Enemy")]
public class SpawnableEnemy : ScriptableObject
{
    [Tooltip("The enemy prefab you want to spawn")]
    public GameObject enemy;
    [Tooltip("The position ID you want said enemy to spawn at (STARTS AT 0)")]
    public int spawnPos;
}
