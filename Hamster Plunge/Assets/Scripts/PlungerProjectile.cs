/*****************************************************************************
// File Name :         PlungerProjectile.cs
// Author :            Nick Grinstead
// Creation Date :     Jan 25, 2023
//
// Brief Description : This class represents the plunger and handles its movement
                       as well as what happens when it hits or misses a target.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungerProjectile : MonoBehaviour
{
    [SerializeField] float maxRange;
    [SerializeField] float moveSpeed;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GrappleMovement grappleMovement;
    [SerializeField] GrappleCancel grappleCancel;

    [SerializeField] float suctionTime;

    GameObject cannonBarrel;
    GameObject pivot;

    ShootPlunger shootPlunger;

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void Awake()
    {
        cannonBarrel = GameObject.FindGameObjectWithTag("ShootBox");
        pivot = GameObject.FindGameObjectWithTag("Pivot");
        shootPlunger = FindObjectOfType<ShootPlunger>();
    }

    /// <summary>
    /// Makes plunger move when the player isn't grappled
    /// </summary>
    private void FixedUpdate()
    {
        if (playerMovement.grappled == false)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // If max range is reached the plunger returns to cannon
            if (Mathf.Abs(Vector3.Distance(transform.position, cannonBarrel.transform.position)) > maxRange)
            {
                Reset();
            }
        }
    }

    /// <summary>
    /// Resets the plunger back to the cannon
    /// </summary>
    public void Reset()
    {
        transform.position = cannonBarrel.transform.position;

        Vector3 newRotation = pivot.transform.eulerAngles;
        newRotation.z -= 45;
        transform.eulerAngles = newRotation;
        
        shootPlunger.ReloadCannon();
    }

    /// <summary>
    /// Handles how the plunger grapples to walls
    /// </summary>
    /// <param name="collision">Collision data</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerMovement.grappled == false && collision.gameObject.CompareTag("Wall"))
        {
            playerMovement.grappled = true;
            grappleMovement.isMoving = true;
            StartCoroutine(LoseSuction());
        }
    }

    /// <summary>
    /// Cause player to detach from a grappled surface after a set amount of time
    /// </summary>
    /// <returns>suctionTime is number of seconds player can stick for</returns>
    private IEnumerator LoseSuction()
    {
        bool initialDelay = false;

        while (true)
        {
            if (initialDelay)
            {
                grappleCancel.Reload();

                StopAllCoroutines();
            }
            else
            {
                initialDelay = true;
            }

            yield return new WaitForSeconds(suctionTime);
        }
    }
}
