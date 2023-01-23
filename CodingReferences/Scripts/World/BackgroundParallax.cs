/*****************************************************************************
// File Name :         BackgroundParallax.cs
// Author :            Christian Young
// Creation Date :     September 28, 2022
//
// Brief Description : This script controls the movement of background
                       objects, providing visual depth
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private float startPos;                         // Reference to object's original position
    private Camera cam;                             // Reference to the main camera
    [SerializeField] private float parallaxFX;      // Value to change how much each object moves
                                                    // Current used values: 0.1, 0.3, 0.7, 0.9

    /// <summary>
    /// Initializes references to main cam and startPos
    /// </summary>
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        startPos = transform.position.x;
    }

    /// <summary>
    /// Moves background object based on parallaxFX value
    /// </summary>
    void FixedUpdate()
    {
        float distance = (cam.transform.position.x * parallaxFX);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
