using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPaw : MonoBehaviour
{
    private Transform player;
    [SerializeField] float maxDist = 5f;
    private bool canAttack = true;
    [SerializeField] float hitForce;
    private Animator anim;
    
    void Start(){
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update(){
        if(Vector2.Distance(player.position, transform.position) < maxDist && canAttack){
            StartCoroutine(pawSwipe());
        }
    }

    IEnumerator pawSwipe (){
        canAttack = false;
        anim.SetTrigger("attack");
        if(AngleDir(transform.position, player.position) > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else{
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        yield return new WaitForSeconds(4f);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Trigger hit");
        if(collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if(/*AngleDir(transform.position, player.position)*/transform.position.x - player.position.x < 0)
            {
                rb.velocity = hitForce*rb.mass*transform.right*-1 + Vector3.up*rb.velocity.y;
            }
            else /*if(AngleDir(transform.position, player.position) < 0)*/
            {
                rb.velocity = hitForce*rb.mass* transform.right + Vector3.up*rb.velocity.y;
            }
            
        }
    }

    public static float AngleDir(Vector2 A, Vector2 B)
    {
        return -A.x * B.y + A.y * B.x;
    }
}
