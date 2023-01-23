using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to just control taking damage from an enemy attack when it connects
*/

public class HitBoxScript : MonoBehaviour
{
    
    public PlayerMovement pm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy_Attack"))
        {
            pm.AttackHit();
        }
    }

    
}
