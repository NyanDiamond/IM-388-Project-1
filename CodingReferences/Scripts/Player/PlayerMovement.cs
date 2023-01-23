using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to contol the various actions the player can make
 * First, controls genric movement such as jumping and running
 * Also controls throwing and using the teleporter
 * As well, it controls attacking and a now depricated combo system which was later scrapped in the final build
*/
public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float jumpForce;
    public float throwPower;
    public GameObject teleporter;
    public GameObject throwPoint;
    public float aimLength = 2f;
    private GroundChecker gr;
    private LineRenderer lr;
    private float movement;
    //player components
    private Animator an;
    private Rigidbody2D rb;
    private PlayerControls pc;
    //variables to keep track of certain aspects
    //private bool jumping = false;
    private bool grounded = true;
    private bool canAttack = true;
    private bool hit = true;
    private int attack = 1;
    //private bool teleporterOut = false;
    private bool isAttacking = false;
    private bool canTP = true;
    private bool isHit = false;
    private bool teleporting = false;
    public float teleportCDTimer;
    private float teleportCD = 2f;
    private Image cooldownIndicator;

    public bool restricted = false;
    private float gravity;
    [SerializeField] private float gravMult;
    
    private Coroutine combo;
    // Start is called before the first frame update

    private void Awake()
    {
        cooldownIndicator = GameObject.FindGameObjectWithTag("Cooldown").GetComponent<Image>();
        pc = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        gr = transform.Find("GroundCheck").GetComponent<GroundChecker>();
        lr = GetComponent<LineRenderer>();
        combo = null;
        gravity = rb.gravityScale;
    }

    private void OnEnable()
    {
        
        pc.Enable();
        pc.Default.Jump.performed += _ => Jump();
        pc.Default.TeleportMK.performed += _ => TeleportMK();
        pc.Default.TeleportCT.performed += _ => TeleportCT();
        pc.Default.Attack.performed += _ => Attack();
        pc.Default.ReturnTeleporter.performed += _ => ReturnTP();
        pc.Default.FastFall.performed += _ => FastFallStart();
        pc.Default.FastFall.canceled += _ => FastFallEnd();
    }

    private void OnDisable()
    {
        pc.Disable();
    }
 
    private void ReturnTP()
	{
        GameObject teleporter = GameObject.FindGameObjectWithTag("Teleporter");
        if (teleporter != null)
        {
            Destroy(teleporter);
            cooldownIndicator.transform.GetChild(0).gameObject.SetActive(true);
            SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
            sp.PlaySound12();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isHit)
        {
            movement = pc.Default.Move.ReadValue<float>();
            //Check to see if the player is moving and set bool accordingly for animations
            if (movement != 0)
            {
                an.SetBool("isRunning", true);
                //if moving left
                if (movement < 0 && !isAttacking)
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                //if moving right
                else if (movement > 0 && !isAttacking)
                    transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
                StartCoroutine(IdleCooldown());
            Move();
        }

        grounded = GroundCheck();
        if (!grounded && rb.velocity.y > 0.1)
        {
            //Debug.Log("Jumping");
            an.SetBool("Jumping", true);
        }
        if (grounded && rb.velocity.y <= 0.1)
        {
            //Debug.Log("Landed");
            an.SetBool("Jumping", false);
            if(!canTP && !teleporting)
            {
                canTP = true;
            }
        }

        Vector2 lookpos = pc.Default.TeleportAiming.ReadValue<Vector2>();
        //Debug.Log(lookpos);
        if(lookpos != Vector2.zero && teleportCD>=teleportCDTimer)
        {
            //Debug.Log("Making line");
            Vector2 currentPos = throwPoint.transform.position;
            Vector2 target = (Vector2)throwPoint.transform.position + lookpos.normalized * aimLength;

            lr.SetPositions(new Vector3[] {currentPos, target});
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }
    }

    private void Move()
    {
        float yVel = rb.velocity.y;
        if (!isHit && !isAttacking)
        {
            rb.velocity = new Vector2(maxSpeed * movement, yVel);
        }
    }

    private void Jump()
    {
        
        if (grounded && !isHit && Time.timeScale!=0)
        {
            //jumping = true;
            //an.SetBool("Jumping", true);
            rb.AddForce(transform.up * jumpForce);
            SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
            sp.PlaySound6();
        }
    }

    private bool GroundCheck()
    {   
        //Debug.Log(gr.grounded);
        return gr.grounded;
        
    }

    private void FastFallStart()
    {
        rb.gravityScale *= gravMult;
    }

    private void FastFallEnd()
    {
        rb.gravityScale = gravity;
    }
    private void TeleportCT()
    {
        if (!restricted && Time.timeScale !=0 )
        {
            GameObject tp = GameObject.FindGameObjectWithTag("Teleporter");
            if (canTP && !isHit && teleportCD >= teleportCDTimer)
            {

                //jumping = true;
                if (tp == null)
                {
                    Vector2 lookPos = pc.Default.TeleportAiming.ReadValue<Vector2>();
                    if (lookPos == Vector2.zero)
                        lookPos = transform.right;
                    tp = Instantiate(teleporter, throwPoint.transform.position, Quaternion.identity);
                    tp.GetComponent<Rigidbody2D>().AddForce(lookPos.normalized * throwPower * tp.GetComponent<Rigidbody2D>().mass);
                    cooldownIndicator.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    Teleport(tp);
                }
            }
        }
    }

    private void TeleportMK()
    {
        if (!restricted && Time.timeScale != 0)
        {
            GameObject tp = GameObject.FindGameObjectWithTag("Teleporter");

            if (canTP && !isHit && teleportCD >= teleportCDTimer)
            {

                //jumping = true;
                if (tp == null)
                {
                    Camera cam = GameObject.FindObjectOfType<Camera>();
                    Vector2 lookPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - throwPoint.transform.position;
                    //create teleporter and add a force to throw it
                    tp = Instantiate(teleporter, throwPoint.transform.position, Quaternion.identity);
                    tp.GetComponent<Rigidbody2D>().AddForce(lookPos.normalized * throwPower * tp.GetComponent<Rigidbody2D>().mass);

                    cooldownIndicator.transform.GetChild(0).gameObject.SetActive(false);
                    //tp.GetComponent<TeleporterBehavior>().Teleported();
                    //teleporterOut = true;
                }
                else
                {
                    Teleport(tp);
                }
            }
        }
    }

    private void Teleport(GameObject tp)
    {
        //tp.GetComponent<TeleporterBehavior>().Teleported();
        if (EnemyCounter.doorClosed)
            teleportCD = 0;
        else
            teleportCD = teleportCDTimer/2;
        StartCoroutine(TeleporterCooldown());
        teleporting = true;
        canTP = false;
        if (tp.transform.parent == null)
        {
            transform.position = tp.transform.position - new Vector3(0.057f, 0.204f, 0f);
        }
        else if (tp.transform.position.y < tp.transform.parent.position.y)
        {
            transform.position = tp.transform.position - Vector3.up;
        }
        else
            transform.position = tp.transform.position;
        EnemyCounter.TeleporterCheck();
        TeleporterBehavior temp = tp.GetComponent<TeleporterBehavior>();
        //if TP is stuck to an enemy, destroy it
        if (temp.attached)
        {
            tp.transform.parent.GetComponent<EnemyHealthBehavior>().instantDeath();
        }
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        StartCoroutine(Grow());
        Destroy(tp);
        SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
        sp.PlaySound11();
        cooldownIndicator.transform.GetChild(0).gameObject.SetActive(true);
        //teleporterOut = false;
        teleporting = false;

    }

    private IEnumerator Grow()
    {
        while (transform.localScale.x < 2f)
        {
            yield return new WaitForSeconds(0.005f);
            transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
        }
    }

    private void Attack()
    {
        if (canAttack && !isHit && Time.timeScale != 0)
        {
            if(combo != null)
                StopCoroutine(combo);
            canAttack = false;
            isAttacking = true;
            //rb.velocity = (Vector2)(transform.right) * 0.4f + new Vector2 (0,rb.velocity.y);
            rb.velocity = Vector2.zero;
            hit = false;
            SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
            an.SetTrigger("Attack2");
            sp.PlaySound13();
            /*switch (attack)
            {
                case 1:
                    an.SetTrigger("Attack1");
                    attack++;
                    sp.PlaySound13();
                    break;
                case 2:
                    an.SetTrigger("Attack2");
                    attack++;
                    sp.PlaySound13();
                    break;
                case 3:
                    an.SetTrigger("Attack3");
                    attack++;
                    sp.PlaySound5();
                    break;
            }*/
        }
    }

    private void AttackEnd()
    {
        isAttacking = false;
        if (hit && attack != 4)
        {
            combo = StartCoroutine(ComboWindow());
        }
        else
        {
            attack = 1;
        }
        canAttack = true;
    }

    public void AttackHit()
    {
        hit = true;
    }
	public void EnterRoom()
	{
        cooldownIndicator.transform.GetChild(0).gameObject.SetActive(true);
	}
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if the player lands while jumping
        /*if (jumping && collision.gameObject.CompareTag("Floor") && transform.Find("Target").position.y > collision.gameObject.transform.position.y)
        {
           
            an.SetTrigger("Land");
            jumping = false;
        }
        if (!canTP && collision.gameObject.CompareTag("Floor") && transform.Find("Target").position.y > collision.gameObject.transform.position.y)
        {
            canTP = true;
        }*/

    }

    IEnumerator IdleCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        {
            if (movement == 0)
                an.SetBool("isRunning", false);
        }
    }

    IEnumerator ComboWindow()
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("Combo Reset");
        attack = 1;
    }

    IEnumerator TeleporterCooldown()
    {
        cooldownIndicator.fillAmount = teleportCD/teleportCDTimer;
        while(teleportCD<teleportCDTimer)
        {
            yield return new WaitForSeconds(0.1f);
            teleportCD += 0.1f;
            cooldownIndicator.fillAmount = teleportCD / teleportCDTimer;
        }
    }

    public void Damaged()
    {
        isHit = true;
        an.SetTrigger("Hit");
        rb.velocity = (-transform.forward) * 0.1f;
    }

    public void Death()
    {
        an.SetTrigger("Death");
        isHit = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    private void OutOfHitstun()
    {
        isHit = false;
    }

}
