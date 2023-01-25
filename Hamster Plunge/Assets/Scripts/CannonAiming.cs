/*****************************************************************************
// File Name :         CannonAiming.cs
// Author :            Nick Grinstead
// Creation Date :     Jan 23, 2023
//
// Brief Description : This class handles the rotation of the cannon as the
                       player aims.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonAiming : MonoBehaviour
{
    Vector3 mousePosition;

    PlayerControls playerControls;
    InputAction aim;

    /// <summary>
    /// Initializes input controls
    /// </summary>
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Default.Enable();
        aim = playerControls.FindAction("Aim");
    }

    /// <summary>
    /// Calls function to rotate cannon every physics update
    /// </summary>
    private void FixedUpdate()
    {
        Rotate();
    }

    /// <summary>
    /// Rotates the cannon to face the mouse cursor
    /// </summary>
    private void Rotate()
    {
        // Getting mouse position
        mousePosition = aim.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;

        // Calculating angle
        Vector2 delta = mousePosition - transform.position;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle - 45);
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
