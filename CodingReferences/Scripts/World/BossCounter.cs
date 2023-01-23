using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is derived from the Enemy Counter and is used to supplement the special aspects of the final boss
*/
public class BossCounter : EnemyCounter
{
    private BossBehavior bb;
    // Start is called before the first frame update
    protected override void Awake()
    {
        enemies = new List<GameObject>();
        count = 0;
        doorsave = new List<GameObject>();
        cameras = new List<GameObject>();
        countChanged = false;

        stealth = false;
        doorClosed = true;
        bb = GameObject.FindObjectOfType<BossBehavior>();
    }
    protected override void LateUpdate()
    {
        if (countChanged && count<=0)
        {
            count = 0;
            countChanged = false;
            bb.EndWave();
        }
        else if (countChanged)
        {

            countChanged = false;
            //you can add indicator of how many enemies are left here if you want
        }
    }
}
