using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootPlunger : MonoBehaviour
{
    Collider2D plungerCollider;

    PlayerControls playerControls;
    InputAction shoot;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Default.Enable();
        shoot = playerControls.FindAction("Shoot");
        shoot.performed += ctx => Shoot(); 
    }

    private void Shoot()
    {

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
