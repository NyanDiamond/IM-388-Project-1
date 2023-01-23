using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBehavior : MonoBehaviour
{
    private GameObject playerObject;
    private PlayerMovement pm;
    private Transform player;
    public float movementSpeed = 3f;
    public float tooCloseRange = 4f;
    public float tooFarRange = 6f;
    public GameObject bullet;
    public GameObject shootPoint;
    public GameObject viewPoint;
    [Tooltip("Degrees of offset for specific bullet sprite to look correct")]
    public float rotationFix;
    [Tooltip("How long is the hitstun of the enemy")]
    public float hitstun;

    //All variables for alarm
    [Tooltip("How far is viewing range")]
    public float viewDistance;
    [Tooltip("How wide is POV (starting from center, IE 90 would be 45 degrees up and 45 degrees down from view")]
    public float viewAngle;
    private bool aware = false;
    private bool alarmSounded = false;
    private bool alarmDestroyed = false;
    private bool stunned = false;
    private bool lookingLeft;
    private float attackCooldown = 0;
    private AlarmUI au;

    
  

    private Rigidbody2D rb;
    private Animator an;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        playerObject = GameObject.Find("Player");
        player = playerObject.transform.Find("Target");
        pm = playerObject.GetComponent<PlayerMovement>();
        StartCoroutine(ChargeAttack());
        au = GetComponent<AlarmUI>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (aware)
        {
            Move();
        }
        else if (!stunned)
        {
            //Debug.Log("Check For Player");
            CheckForPlayer();
        }
    }


    void LateUpdate()
    {
        if (aware)
        {
            if (player.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                lookingLeft = true;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                lookingLeft = false;
            }

            if (attackCooldown >= 2f && LOS())
            {
                an.SetBool("Attacking", true);
            }
        }
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
            if(Vector2.Angle(viewPoint.transform.right, playerDirection) < viewAngle/2f)
            {
                //check for inbetween
                bool seen = LOS();
                if(seen)
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
        else if (Vector2.Distance(player.position, transform.position) < viewDistance+2f)
        {
            //Debug.Log("In Warning Distance");
            if(Vector2.Angle(transform.right, playerDirection) < viewAngle / 2f)
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
        RaycastHit2D ray = Physics2D.Raycast(viewPoint.transform.position, playerDirection, viewDistance);
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

    private void Attacked()
    {
        attackCooldown = 0;
        an.SetBool("Attacking", false);
    }

    private void RangedAttack()
    {
        float angle = Mathf.Atan2(shootPoint.transform.position.y - player.position.y, shootPoint.transform.position.x - player.position.x) * Mathf.Rad2Deg + rotationFix;
        Instantiate(bullet, shootPoint.transform.position, Quaternion.Euler(0,0,angle));
    }

    private void Move()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        //Debug.Log(distance);
        if (distance > tooFarRange)
        {
           // Debug.Log("Too far");
            rb.velocity = new Vector2(movementSpeed * transform.right.x, rb.velocity.y);
            an.SetBool("Walking", true);
            an.SetBool("Forward", true);
        }
        else if (distance < tooCloseRange)
        {
           // Debug.Log("Too close");
            rb.velocity = new Vector2(movementSpeed / 2 * -transform.right.x, rb.velocity.y);
            an.SetBool("Walking", true);
            an.SetBool("Forward", false);
        }
        else if (distance > tooCloseRange && distance < tooFarRange)
        {
            //Debug.Log("In range");
            rb.velocity = new Vector2(0, rb.velocity.y);
            an.SetBool("Walking", false);
            an.SetBool("Forward", true);
        }
    }

    public void StealthBreak()
    {
        aware = true;
        alarmSounded = true;
        au.AlarmRaised();
        StartCoroutine(DeleteAlarm());
    }

    private void Alarm()
    {
        EnemyCounter.StealthBreak();
        au.AlarmRaised();
    }

    public void TeleporterCheck()
    {
        if (aware)
        {

            //Debug.Log("Teleporter Check");
            if (lookingLeft && player.position.x > transform.position.x)
            {
                //Debug.Log("Dazed");
                aware = false;
                stunned = true;
                StartCoroutine(Dazed());
            }
            else if (!lookingLeft && player.position.x < transform.position.x)
            {
                //Debug.Log("Dazed");
                aware = false;
                stunned = true;
                StartCoroutine(Dazed());
            }

        }
    }

    private IEnumerator Dazed()
    {
        yield return new WaitForSeconds(1f);
        aware = true;
        stunned = false;
    }

    public void Damaged()
    {
        StopCoroutine(HitStun());
        StartCoroutine(HitStun());
    }

    public void Dead()
    {
        aware = false;
        if(!alarmDestroyed)
        {
            au.Safe();
        }
        StopAllCoroutines();
        
    }

    private IEnumerator HitStun()
    {
        aware = false;
        stunned = true;
        yield return new WaitForSeconds(hitstun);
        stunned = false;
    }

    private IEnumerator ChargeAttack()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            if(aware && Vector2.Distance(player.position, transform.position)<tooFarRange && LOS())
            {
                attackCooldown += 0.1f;
            }
            else if (attackCooldown>0)
            {
                attackCooldown -= 0.01f;
            }
        }
    }

    private IEnumerator RaiseAlarm()
    {
        float awareness = 0;
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            awareness += 0.1f;
            if(aware && !alarmSounded)
            {
                if (awareness >= 1f)
                    Alarm();
            }
        }
    }

    private IEnumerator DeleteAlarm()
    {
        yield return new WaitForSeconds(2f);
        Destroy(au);
        alarmDestroyed = true;
    }
  
}
