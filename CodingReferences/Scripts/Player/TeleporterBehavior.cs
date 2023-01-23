using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to how the teleporter reacts to hitting certain objects
*/
public class TeleporterBehavior : MonoBehaviour
{

    public bool attached = false;
    public float bounceVelocityDecay;

    private Rigidbody2D rig;
    private PlayerMovement playerMovement;
    private Vector2 currentVel;
    private bool hit = false;
    private int bounceNum = 0;
    public ArrowBehavior ab;
    private bool firstTime = true;
    //private Text tpStatus;
    private void Start()
    {
        ab = GameObject.FindObjectOfType<ArrowBehavior>();
        rig = GetComponent<Rigidbody2D>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //tpStatus = GameObject.FindGameObjectWithTag("TPStatus").GetComponent<Text>();

        //tpStatus.text = "Teleporter Status: THROWN";
    }
    private void Update()
    {
        if (attached)
        {
            transform.position = transform.parent.Find("Center Point Soldier").position;
        }
        else if (!hit)
		{
            currentVel = GetComponent<Rigidbody2D>().velocity;
        }
    }
    /*public void Teleported()
	{
        tpStatus.text = "Teleporter Status: READY";
    }
    */
    //private void OnDestroy()
    //{
    //    tpStatus.text = "Teleporter Status: READY";
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        hit = true;
        /*if (collision.gameObject.CompareTag("Floor"))
        {
            Vector3 safescale = transform.lossyScale;
            GetComponent<CircleCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Static;
            transform.parent = collision.gameObject.transform;
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(safescale.x * 1 / transform.parent.localScale.x, safescale.y * 1 / transform.parent.localScale.y);
            
        }*/
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //tpStatus.text = "Teleporter Status: STUCK";
            GetComponent<CircleCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            attached = true;
            transform.parent = collision.gameObject.transform;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            transform.position = transform.parent.Find("Center Point Soldier").position;
        }
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Turret") || collision.gameObject.CompareTag("Shield"))
        {
            //Debug.Log("Hit Wall!");
            //reverses the direction of the teleporter and
            //gives it a speed based on throw power and the number of bouces since it was thrown
            bounceNum++;
            rig.AddForce(new Vector2(-currentVel.x, currentVel.y) * playerMovement.throwPower * rig.mass / 8 / Mathf.Pow(bounceNum, bounceVelocityDecay));

        }
        hit = false;

        
    }

    private void OnBecameVisible()
    {
        if (!firstTime)
            ab.TurnOff();
        
    }

    private void OnBecameInvisible()
    {
        firstTime = false;
        if(transform != null)
            ab.TeleporterOffScreen();
    }

    private void OnDestroy()
    {
        if(!firstTime)
            ab.TurnOff();
    }
}
