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

    private void Shoot()
    {
        if (!hasShot)
        {
            hasShot = true;

            transform.parent = null;
            //GetComponent<SpriteRenderer>().enabled = false;

            // Getting mousePosition
            mousePosition = aim.ReadValue<Vector2>();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;

            // Calculating direction
            Vector2 travelVector = mousePosition - transform.position;
            float angle = Mathf.Atan2(travelVector.y, travelVector.x) * Mathf.Rad2Deg;
            travelVector = Vector3.Normalize(travelVector);

            //PlungerProjectile newPlunger = Instantiate(plungerProjectile).GetComponent<PlungerProjectile>();
            plungerProjectile.enabled = true;
            boxCollider.enabled = true;
            //plungerProjectile.SetTravelDirection(travelVector);
        }
    }

    public void ReloadCannon()
    {
        transform.SetParent(cannonBarrel.transform);
        plungerProjectile.enabled = false;
        boxCollider.enabled = false;
        hasShot = false;
        //GetComponent<SpriteRenderer>().enabled = true;
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
