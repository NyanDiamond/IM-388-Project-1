using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to contol the bullets the enemies shoot
*/
public class BulletBehavior : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;

    private void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
        SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
        sp.PlaySound5();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Player_Attack") || collision.gameObject.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }

}
