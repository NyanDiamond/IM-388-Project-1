using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to contol multiple aspects of the enemy
 * First it uses A* pathing in order to find the player
 * It also has a line of sight code that will activate the enemies tracking script once it is tripped
 * This script also controls when the enemy will attack and how far away they must be in order to attack
 * Works in conjunction with the Enemy Container in order to sound the alarm
*/
public class ExperimentalEnemyMovement : MonoBehaviour
{
    private GameObject playerObject;
    //private PlayerMovement pm;
    private Transform player;
    public float movementSpeed = 3f;
    //public float tooCloseRange = 4f;
    public float tooFarRange = 6f;
    public float attackSpeed = 2f;
    public GameObject bullet;
    public GameObject shootPoint;
    public GameObject viewPoint;
    public LayerMask layermask;
    public Transform centerPoint;
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
    private AlarmUI au;

    private bool stunned = false;
    private bool grounded = true;
    
    private float attackCooldown = 0;
    

    //All variables for pathfinding
    public float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;

    private Coroutine stun;

    private Rigidbody2D rb;
    private Animator an;
    private GroundChecker gr;

    [SerializeField] bool lookingLeft;
    [SerializeField] bool stealthMoving = false;
    [SerializeField] float stealthWalkTime;
    [SerializeField] float stealthPauseTime;
    private bool autoMovement = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        playerObject = GameObject.Find("Player");
        player = playerObject.transform.Find("Target");
        //pm = playerObject.GetComponent<PlayerMovement>();
        StartCoroutine(ChargeAttack());
        au = GetComponent<AlarmUI>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.2f);
        gr = transform.Find("GroundCheck").GetComponent<GroundChecker>();
        if(stealthMoving)
            StartCoroutine(StealthPathing());
    }

	private void OnDisable()
	{
        StopAllCoroutines();
	}
	void UpdatePath()
    {
        if(seeker.IsDone())
        seeker.StartPath(centerPoint.position, player.position, OnPathComplete);
        currentWaypoint = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.bodyType != RigidbodyType2D.Static)
        {
            if (path != null)
            {
                if (currentWaypoint >= path.vectorPath.Count)
                {
                    reachedEndOfPath = true;
                }
                else
                {
                    reachedEndOfPath = false;
                }
            }
            if (aware)
            {
                Move();
            }
            else if (!stunned)
            {
                //Debug.Log("Check For Player");
                CheckForPlayer();
                if (stealthMoving)
                    AutoMove();
            }
            else if (!aware && stunned)
            {
                float velY = rb.velocity.y;
                rb.velocity = new Vector2(0, velY);
                
            }
        }
    }

    void AutoMove()
    {
        if(stunned)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        else
        {
            if(autoMovement)
            {
                an.SetBool("Walking", true);
                an.SetBool("Forward", true);
                rb.velocity = new Vector2(movementSpeed * transform.right.x, rb.velocity.y);
            }
            else
            {
                an.SetBool("Walking", false);
                an.SetBool("Forward", true);
                rb.velocity = Vector2.zero;
            }
            
        }
    }

    void LateUpdate()
    {
        grounded = GroundCheck();
        /*if (!grounded && rb.velocity.y > 0)
        {
            //Debug.Log("Jumping");
            an.SetBool("Jumping", true);
        }
        if (grounded && rb.velocity.y <= 0)
        {
            //Debug.Log("Landed");
            an.SetBool("Jumping", false);
        }
        */
            if (aware)
        {
            //if (player.position.x < transform.position.x)
            //{
            //    transform.rotation = Quaternion.Euler(0, 180, 0);
            //    lookingLeft = true;
            //}
            //else
            //{
            //    transform.rotation = Quaternion.Euler(0, 0, 0);
            //    lookingLeft = false;
            //}

            if (attackCooldown >= attackSpeed && LOS() && Vector2.Distance(player.position, centerPoint.position) <= tooFarRange) 
            {
                an.SetBool("Attacking", true);
            }
        }
    }

    bool GroundCheck()
    {
        return gr.grounded;
    }

    void CheckForPlayer()
    {
        Vector2 playerDirection = (player.position - viewPoint.transform.position).normalized;
        //Debug.Log(playerDirection);
        //in view distance
        if (Vector2.Distance(player.position, centerPoint.position) < viewDistance)
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
        else if (Vector2.Distance(player.position, centerPoint.position) < viewDistance + 2f)
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

    private void Attacked()
    {
        attackCooldown = 0;
        an.SetBool("Attacking", false);
    }

    private void RangedAttack()
    {
        float angle = Mathf.Atan2(shootPoint.transform.position.y - player.position.y, 
            shootPoint.transform.position.x - player.position.x) * Mathf.Rad2Deg + rotationFix;
        Instantiate(bullet, shootPoint.transform.position, Quaternion.Euler(0, 0, angle));
    }

    private void Move()
    {
        float distance = Vector2.Distance(player.position, centerPoint.position);
        if(stunned)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if ((distance < tooFarRange) && LOS())
        {
            //Debug.Log("Has LOS");
            rb.velocity = new Vector2(0, rb.velocity.y);
            an.SetBool("Walking", false);
            an.SetBool("Forward", true);
            FacePlayer();
        }
        else
        {
            //Debug.Log("Moving");
            an.SetBool("Walking", true);
            an.SetBool("Forward", true);

            float wayPointDistance = Vector2.Distance(centerPoint.position, path.vectorPath[currentWaypoint]);
            if(wayPointDistance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            //Debug.Log(direction);
            
            
            

            //If not going to jump
            if(direction.y < 0.9)
            {
                rb.velocity = new Vector2(movementSpeed * transform.right.x, rb.velocity.y);
                if (direction.x < 0)
            {
                //Debug.Log("Look left");
                transform.rotation = Quaternion.Euler(0, 180, 0);
                lookingLeft = true;
            }
            else if (direction.x > 0)
            {
                //Debug.Log("Look Right");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                lookingLeft = false;
            }
            }
            //Will Jump
            else if (direction.y>0.98)
            {
                float yVel = rb.velocity.y;
                //horizontal movement speed slowed down drastically when jumping, but still somewhat there
                rb.velocity = new Vector2(movementSpeed * transform.right.x / 10, yVel);
                Jump();
                FacePlayer();
            }
            else
            {
                float yVel = rb.velocity.y;
                //horizontal movement speed slowed down drastically when jumping, but still somewhat there
                rb.velocity = new Vector2(movementSpeed * transform.right.x, yVel);
                Jump();
                FacePlayer();
            }
        }
        
        //Debug.Log(distance);
        //if (distance > tooFarRange)
        //{
        //    // Debug.Log("Too far");
        //    rb.velocity = new Vector2(movementSpeed * transform.right.x, rb.velocity.y);
        //    an.SetBool("Walking", true);
        //    an.SetBool("Forward", true);
        //}
        //else if (distance < tooCloseRange)
        //{
        //    // Debug.Log("Too close");
        //    rb.velocity = new Vector2(movementSpeed / 2 * -transform.right.x, rb.velocity.y);
        //    an.SetBool("Walking", true);
        //    an.SetBool("Forward", false);
        //}
        //else if (distance > tooCloseRange && distance < tooFarRange)
        //{
        //    //Debug.Log("In range");
        //    rb.velocity = new Vector2(0, rb.velocity.y);
        //    an.SetBool("Walking", false);
        //    an.SetBool("Forward", true);
        //}
    }

    private void FacePlayer()
    {
        //Debug.Log("called face player");
        if (player.position.x < centerPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            lookingLeft = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            lookingLeft = false;
        }
    }

    private void Jump()
    {
        float jumpForce = 260;
        if (grounded && aware && Vector2.Distance(centerPoint.position, player.position) > 2)
        {
            //jumping = true;
            //an.SetTrigger("Jump");
            rb.AddForce(transform.up * jumpForce);
        }
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void StealthBreak()
    {
        aware = true;
        alarmSounded = true;
        if(au == null)
        {
            au = GetComponent<AlarmUI>();
        }
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
            if (lookingLeft && player.position.x > centerPoint.position.x)
            {
                //Debug.Log("Dazed");
                aware = false;
                stunned = true;
                StartCoroutine(Dazed());
            }
            else if (!lookingLeft && player.position.x < centerPoint.position.x)
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
        if (stun != null)
            StopCoroutine(stun);
        stun = StartCoroutine(HitStun());
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

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumping && collision.gameObject.CompareTag("Floor") && transform.Find("Target").position.y > collision.gameObject.transform.position.y)
        {
            //Debug.Log("landed");
            //an.SetTrigger("Land");
            jumping = false;
        }
    }
    */

    private IEnumerator HitStun()
    {
        aware = false;
        stunned = true;
        yield return new WaitForSeconds(hitstun);
        stunned = false;
        aware = true;
    }

    private IEnumerator ChargeAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (aware && LOS())
            {
                attackCooldown += 0.1f;
            }
            else if (attackCooldown > 0)
            {
                attackCooldown -= 0.01f;
            }
        }
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
                if (awareness >= 0.5f)
                    Alarm();
            }
        }
    }

    private IEnumerator DeleteAlarm()
    {
        yield return new WaitForSeconds(1f);
        Destroy(au);
        alarmDestroyed = true;
    }

    private IEnumerator StealthPathing()
    {
        while(!aware)
        {
            yield return new WaitForSeconds(stealthWalkTime);
            autoMovement = false;
            yield return new WaitForSeconds(stealthPauseTime);
            autoMovement = true;
            if(lookingLeft)
            {
                lookingLeft = false;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                lookingLeft = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            
        }
    }
}

