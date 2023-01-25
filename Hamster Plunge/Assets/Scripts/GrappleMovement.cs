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

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

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

            if (Mathf.Abs(Vector3.Distance(plungerProjectile.transform.position,
                shootBox.transform.position)) <= 0.25f)
            {
                isMoving = false;
                rb2d.velocity = new Vector2(0, 0);
            }
        }
    }

}
