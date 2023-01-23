using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to have an arrow point towards a thrown teleporter if it manages to get off screen
*/
public class ArrowBehavior : MonoBehaviour
{
    private GameObject arrow;
    private Transform teleporter;
    public float angleOffset = 90f;
    // Start is called before the first frame update
    void Start()
    {
        arrow = GameObject.FindGameObjectWithTag("Arrow");
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (teleporter != null)
        {
            Vector2 dir = (teleporter.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - angleOffset);
        }
    }

    public void TeleporterOffScreen()
    {
        
        teleporter = GameObject.FindGameObjectWithTag("Teleporter").transform;
        if(teleporter!=null)
            arrow.SetActive(true);
    }

    public void TurnOff()
    {
        if(arrow!=null)
            arrow.SetActive(false);
        teleporter = null;
    }
}
