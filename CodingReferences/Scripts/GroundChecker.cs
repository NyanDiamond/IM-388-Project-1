using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to keep track of when an entity is on the ground or not
*/
public class GroundChecker : MonoBehaviour
{
    public bool grounded = true;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor") /*|| collision.CompareTag("Generator")*/)
        {
            grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor") /*|| collision.CompareTag("Generator")*/)
        {
            grounded = false;
        }
    }

    
}
