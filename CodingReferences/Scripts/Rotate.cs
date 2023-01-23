using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Personal Project
 * This code is simply used to allow any object to rotate in any direction inputed, used and coppied over through multiple ones of my projects due to how generic it is
*/
public class Rotate : MonoBehaviour
{
    [SerializeField] float xSpeed, ySpeed, zSpeed;
    void Update()
    {
        transform.Rotate(
             xSpeed * Time.deltaTime,
             ySpeed * Time.deltaTime,
             zSpeed * Time.deltaTime
        );
    }
}
