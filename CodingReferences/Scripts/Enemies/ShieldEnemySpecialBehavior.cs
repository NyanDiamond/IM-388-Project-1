using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This is a quick script to destroy the shield of the shield enemy when it dies
*/
public class ShieldEnemySpecialBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount<=1)
        {
            Destroy(gameObject);
        }
    }
}
