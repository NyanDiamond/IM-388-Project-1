using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to control the turrets behavior (shooting at the player at fixed intervals, locating the player to shoot, etc)
*/
public class TurretBehavior : MonoBehaviour
{
    public GameObject bullet;
    public GameObject shootPoint;
    public float viewDistance = 8f;
    public float attackSpeed = 4f;
    public float rotationFix;
    public float viewAngle;
    public LayerMask layermask;
    private float attackCooldown = 0;
    private bool active = false;

    private GameObject playerObject;
    private Transform player;
    private Vector2 lastKnownPosition;

    private Animator an;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        player = playerObject.transform.Find("Target");
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            FacePlayer();
            if(attackCooldown>attackSpeed)
            {
                an.SetBool("Attack", true);
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Activate()
    {
        an.SetBool("Spawned", true);
    }

    public bool IsActive()
	{
        if (active)
		{
            return true;
		}
		else
		{
            return false;
		}
	}

    private void Activated()
    {
        active = true;
        StartCoroutine(ChargeAttack());
    }

    private void Attacked()
    {
        attackCooldown = 0;
        an.SetBool("Attack", false);
    }

    private void Attack()
    {
        Vector2 target;
        if(LOS())
        {
            target = player.position;
            lastKnownPosition = player.position;
        }
        else
        {
            target = lastKnownPosition;
        }
        float angle = Mathf.Atan2(shootPoint.transform.position.y - target.y,
            shootPoint.transform.position.x - target.x) * Mathf.Rad2Deg + rotationFix;
        Instantiate(bullet, shootPoint.transform.position, Quaternion.Euler(0, 0, angle));
    }

    private void FacePlayer()
    {
        //Debug.Log("called face player");
        if (player.position.x < transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private bool LOS()
    {
        Vector2 playerDirection = (player.position - shootPoint.transform.position).normalized;
        RaycastHit2D ray = Physics2D.Raycast(shootPoint.transform.position, playerDirection, viewDistance, layermask);
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

    private IEnumerator ChargeAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (LOS())
            {
                attackCooldown += 0.1f;
            }
            else if (attackCooldown > 0)
            {
                attackCooldown -= 0.01f;
            }
        }
    }
}
