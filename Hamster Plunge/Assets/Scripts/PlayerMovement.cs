using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Initialize all variables
    [SerializeField] [Tooltip("Rate at which the ball picks up speed in the direction pressed")] float acceleration;
    [SerializeField] [Tooltip("Maximum horizontal velocity (kicks in whenever grounded")] float maxVelocity;
    float input;
    [SerializeField] [Tooltip("Modifier at which the ball rotates based on the velocity")] float rotationSpeed;
    public bool grappled = false;
    Rigidbody2D rb;
    PlayerControls pc;
    GroundChecker gc;
    Transform ball;
    Animator an;
    Transform hamster;
    //[SerializeField] Sprite hamsterIdle;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = new PlayerControls();
        gc = GetComponent<GroundChecker>();
        ball = transform.GetChild(0);
        hamster = transform.GetChild(3);
        an = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        pc.Enable();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //rotate the ball based on speed
        ball.Rotate(Vector3.forward, -rb.velocity.x * rotationSpeed);
        //no matter what get input for animation use
        input = pc.Default.Horizontal.ReadValue<float>();
        //Adjust Hamster Animations
        HamsterAdjust();
        //If on the ground and not grappled move
        if(!grappled && gc.grounded && input!=0)
            Move();
        //if player is grounded and not on a grapple
        if (gc.grounded && !grappled)
            Clamp();
    }

    //This method is used to control the horizontal momentum of the hamster ball
    void Move()
    {
        rb.AddForce(new Vector2(input * acceleration * rb.mass, 0));
    }
    //This method is used to clamp the horizontal velocity to avoid game breaking speeds
    void Clamp()
    {
        rb.velocity = new Vector2 (Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity), rb.velocity.y);
    }

    //This method is to adjust the hamster's animations
    void HamsterAdjust()
    {
        //If there is no input from the player
        if(input==0)
        {
            an.SetBool("Input", false);
            //moving left
            if(rb.velocity.x < 0)
            {
                an.SetBool("BallMoving", true);
                hamster.rotation = Quaternion.Euler(0, 180, 0);
            }
            //moving right
            else if (rb.velocity.x>0)
            {
                an.SetBool("BallMoving", true);
                hamster.rotation = Quaternion.Euler(0, 0, 0);
            }
            //not moving
            else
            {
                an.SetBool("BallMoving", false);
                //hamster.gameObject.GetComponent<SpriteRenderer>().sprite = hamsterIdle;
            }
        }
        //input from player
        else
        {
            an.SetBool("Input", true);
            an.SetBool("BallMoving", true);
            if(input<0)
            {
                hamster.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                hamster.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
