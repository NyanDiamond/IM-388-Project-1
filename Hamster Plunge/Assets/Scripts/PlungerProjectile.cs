using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungerProjectile : MonoBehaviour
{
    [SerializeField] float maxRange;
    [SerializeField] float moveSpeed;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GrappleMovement grappleMovement;

    GameObject cannonBarrel;
    //GameObject pivot;

    ShootPlunger shootPlunger;

    Vector2 travelDirection;

    private void Awake()
    {
        cannonBarrel = GameObject.FindGameObjectWithTag("ShootBox");
        //pivot = GameObject.FindGameObjectWithTag("Pivot");
        shootPlunger = FindObjectOfType<ShootPlunger>();
    }

    private void FixedUpdate()
    {
        if (playerMovement.grappled == false)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            if (Mathf.Abs(Vector3.Distance(transform.position, cannonBarrel.transform.position)) > maxRange)
            {
                transform.position = cannonBarrel.transform.position;
                //transform.rotation = pivot.transform.rotation;
                transform.eulerAngles = new Vector3(0, 0, -45);
                shootPlunger.ReloadCannon();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            playerMovement.grappled = true;
            grappleMovement.isMoving = true;
        }
    }
}
