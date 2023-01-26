/*****************************************************************************
// File Name :         AhootPlunger.cs
// Author :            Nick Grinstead
// Creation Date :     Jan 23, 2023
//
// Brief Description : This class shoots the plunger in the direction the player
                       is aiming when they give an input.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootPlunger : MonoBehaviour
{
    [SerializeField] PlungerProjectile plungerProjectile;

    PlayerControls playerControls;
    InputAction shoot;
    InputAction aim;

    GameObject cannonBarrel;
    BoxCollider2D boxCollider;

    Vector3 mousePosition;

    public bool hasShot = false;

    /// <summary>
    /// Initializes input and some variables
    /// </summary>
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Default.Enable();
        shoot = playerControls.FindAction("Shoot");
        aim = playerControls.FindAction("Aim");
        shoot.performed += ctx => Shoot();

        cannonBarrel = GameObject.FindGameObjectWithTag("ShootBox");
        transform.SetParent(cannonBarrel.transform);

        boxCollider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Called when the player presses a certain button to shoot the plunger
    /// </summary>
    private void Shoot()
    {
        // Player can't shoot if plunger has already been shot
        if (!hasShot)
        {
            hasShot = true;

            transform.parent = null;

            // Getting mousePosition
            mousePosition = aim.ReadValue<Vector2>();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;

            // Calculating direction
            Vector2 travelVector = mousePosition - transform.position;
            float angle = Mathf.Atan2(travelVector.y, travelVector.x) * Mathf.Rad2Deg;
            travelVector = Vector3.Normalize(travelVector);

            plungerProjectile.enabled = true;
            boxCollider.enabled = true;
        }
    }

    /// <summary>
    /// Called by other classes to reset plunger cannon
    /// </summary>
    public void ReloadCannon()
    {
        transform.SetParent(cannonBarrel.transform);
        plungerProjectile.enabled = false;
        boxCollider.enabled = false;
        hasShot = false;
    }

    /// <summary>
    /// Enabling playerControls
    /// </summary>
    private void OnEnable()
    {
        playerControls.Enable();
    }

    /// <summary>
    /// Disabling playerControls
    /// </summary>
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
