using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingArm : MonoBehaviour
{
    bool reverse = false;
    [SerializeField] float force;
    [SerializeField] float angle;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Rotation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Rotation()
    {
        while (true)
        {
            //Debug.Log("rotate");
            yield return new WaitForSeconds(0.1f/speed);
            if (reverse)
            {
                transform.Rotate(new Vector3(0, 0, -0.1f));
                //Debug.Log(transform.localRotation.eulerAngles.z);
                if (transform.localRotation.eulerAngles.z <= 360 - angle && transform.localRotation.eulerAngles.z > 360 - angle - 1) 
                {
                    reverse = false;
                    yield return new WaitForSeconds(0.1f/speed);
                }
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 0.1f));
                //Debug.Log(transform.rotation.eulerAngles.z);
                if (transform.localRotation.eulerAngles.z >= angle && transform.localRotation.eulerAngles.z <= angle+1)
                {
                    reverse = true;
                    yield return new WaitForSeconds(0.1f/speed);
                }
            }

        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D tempRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (reverse)
            {
                tempRB.velocity = force * tempRB.mass * Vector3.right * -1 + Vector3.up * tempRB.velocity.y;
            }
            else
            {
                tempRB.velocity = force * tempRB.mass * Vector3.right + Vector3.up * tempRB.velocity.y;
            }
        }
    }
}
