/*****************************************************************************
// File Name :         GrappleMovement.cs
// Author :            Nick Grinstead
// Creation Date :     Jan 25, 2023
//
// Brief Description : This class makes the player move toward the plunger when
                       grappled.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleMovement : MonoBehaviour
{
    [SerializeField] GameObject plungerProjectile;
    [SerializeField] GameObject shootBox;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] float forceMagnitude;

    Rigidbody2D rb2d;
    public bool isMoving = false;

    /// <summary>
    /// Initializing a variable
    /// </summary>
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Moves player when grappled then reduces force applied once player has
    /// reached the plunger
    /// </summary>
    private void FixedUpdate()
    {
        if (playerMovement.grappled == true)
        {
            Vector2 travelVector = plungerProjectile.transform.position - shootBox.transform.position;
            travelVector = Vector3.Normalize(travelVector);

            if (isMoving)
            {
                rb2d.AddForce(travelVector * forceMagnitude);
            }
            else
            {
                rb2d.AddForce(travelVector * forceMagnitude * 0.5f);
            }

            // Slows player upon reaching plunger
            if (Mathf.Abs(Vector3.Distance(plungerProjectile.transform.position,
                shootBox.transform.position)) <= 0.25f)
            {
                isMoving = false;
                rb2d.velocity = new Vector2(0, 0);
            }
        }
    }

}
