/*****************************************************************************
// File Name :         GrappleCancelcs
// Author :            Nick Grinstead
// Creation Date :     Jan 26, 2023
//
// Brief Description : This class allows the player to reset the grapple by 
                       giving an input.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleCancel : MonoBehaviour
{
    PlayerControls playerControls;
    InputAction reload;

    [SerializeField] PlungerProjectile plungerProjectile;
    [SerializeField] GrappleMovement grappleMovement;
    [SerializeField] PlayerMovement playerMovement;

    /// <summary>
    /// Initializing input
    /// </summary>
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Default.Enable();
        reload = playerControls.FindAction("Reload");
        reload.performed += ctx => Reload();
    }

    /// <summary>
    /// Calls other scripts when input is given to ungrapple the player
    /// </summary>
    public void Reload()
    {
        grappleMovement.isMoving = false;
        playerMovement.grappled = false;
        plungerProjectile.Reset();
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
