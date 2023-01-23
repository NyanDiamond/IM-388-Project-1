using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to control movable platforms by having them move to a set of multiple points until they reach the end, in which case they will then move in the opposite direction along those same points
*/
public class MovingPlatform : MonoBehaviour
{
    [Tooltip("Start with its starting position")]
    [SerializeField] Vector2[] movePoints;
    [SerializeField] float speed;

    private bool forward = true;
    private int movePoint = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.localPosition, movePoints[movePoint])<0.001)
        {
            if(forward)
            {
                if(movePoint+1>=movePoints.Length)
                {
                    forward = false;
                    movePoint--;
                }
                else
                {
                    movePoint++;
                }
            }
            else
            {
                if(movePoint <= 0)
                {
                    forward = true;
                    movePoint++;
                }
                else
                {
                    movePoint--;
                }
            }
        }
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, movePoints[movePoint], speed * Time.deltaTime);
    }
}
