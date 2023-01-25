using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPaw : MonoBehaviour
{
    private Transform player;
    public float maxDist = 5f;
    private bool canAttack = true;
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


        yield return new WaitForSeconds(6f);
        canAttack = true;
    }
}
