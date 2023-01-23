using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ian Connors
 * Project: F.E.I.N.T
 * This code is used to contol the alarm coloring and disapearing in terms of the stealth mechanics
*/
public class CameraBehavior : MonoBehaviour
{
    //All variables for alarm
    [Tooltip("How far is viewing range")]
    public float viewDistance;
    [Tooltip("How wide is POV (starting from center, IE 90 would be 45 degrees up and 45 degrees down from view")]
    public float viewAngle;
    private bool aware = false;
    private bool alarmSounded = false;
    private bool alarmDestroyed = false;
    private AlarmUI au;

    [Tooltip("Will the camera move")]
    [SerializeField] bool movable = false;
    [Tooltip("How long will it move in one direction (MUST HAVE MOVABLE = TRUE)")]
    [SerializeField] float moveTime = 0f;
    [Tooltip("How long will it wait to turn around once its moved enough (MUST HAVE MOVABLE = TRUE)")]
    [SerializeField] float turnTime = 0f;

    //viewpoint right infront of camera
    [SerializeField] GameObject viewPoint;
    //Movement Speed
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] LayerMask layermask;
    private Rigidbody2D rb;
    private GameObject playerObject;
    private Transform player;
    private Animator an;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObject = GameObject.Find("Player");
        player = playerObject.transform.Find("Target");
        an = GetComponent<Animator>();
        au = GetComponent<AlarmUI>();
        StartCoroutine(AutomaticMovement());
    }
    private void Update()
    {
        CheckForPlayer();
    }

    void CheckForPlayer()
    {
        Vector2 playerDirection = (player.position - viewPoint.transform.position).normalized;
        //Debug.Log(playerDirection);
        //in view distance
        if (Vector2.Distance(player.position, transform.position) < viewDistance)
        {
            //Debug.Log("In Distance")
            //Debug.Log(Vector2.Angle(viewPoint.transform.right, playerDirection));
            //in field of view
            if (Vector2.Angle(viewPoint.transform.right, playerDirection) < viewAngle / 2f)
            {
                //check for inbetween
                bool seen = LOS();
                if (seen)
                {
                    aware = true;
                    au.Spotted();
                    StartCoroutine(RaiseAlarm());
                }
                else
                {
                    au.Warning();
                }

            }
        }
        //further than view distance but in extended view cone (walls don't matter here)
        else if (Vector2.Distance(player.position, transform.position) < viewDistance + 2f)
        {
            //Debug.Log("In Warning Distance");
            if (Vector2.Angle(transform.right, playerDirection) < viewAngle / 2f)
            {
                au.Warning();
            }
        }
        else
        {
            //Debug.Log("Not in range");
            au.Safe();
        }
    }

    private bool LOS()
    {
        Vector2 playerDirection = (player.position - viewPoint.transform.position).normalized;
        RaycastHit2D ray = Physics2D.Raycast(viewPoint.transform.position, playerDirection, viewDistance, layermask);
        if (ray.collider != null)
        {
            //Debug.Log(ray.collider.gameObject.tag);
            if (ray.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        return false;
    }

    private IEnumerator RaiseAlarm()
    {
        float awareness = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            awareness += 0.1f;
            if (aware && !alarmSounded)
            {
                if (awareness >= 1f)
                    Alarm();
            }
        }
    }

    private void Alarm()
    {
        EnemyCounter.StealthBreak();
        if(!alarmDestroyed)
        au.AlarmRaised();
    }

    public void StealthBreak()
    {
        aware = true;
        alarmSounded = true;
        au.AlarmRaised();
        StartCoroutine(DeleteAlarm());
    }

    public void Dead()
    {
        aware = false;
        if (!alarmDestroyed)
        {
            au.Safe();
        }
        StopAllCoroutines();

    }

    private IEnumerator DeleteAlarm()
    {
        yield return new WaitForSeconds(2f);
        Destroy(au);
        alarmDestroyed = true;
    }

    private IEnumerator AutomaticMovement()
    {
        while (!aware && movable)
        {
            rb.velocity = transform.right * movementSpeed;
            an.SetBool("Moving", true);
            yield return new WaitForSeconds(moveTime);
            rb.velocity = Vector2.zero;
            an.SetBool("Moving", false);
            yield return new WaitForSeconds(turnTime);
            Vector3 angles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(angles.x, angles.y + 180, angles.z);
        }
    }
}
