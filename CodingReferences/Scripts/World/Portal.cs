using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Ian Connors
 * Project: F.E.I.N.T
 * This code is used to have the player jump between matching portals on a small cooldown
*/
public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    public Portal destination;
    public bool set;
    private Coroutine temp;

	private void Start()
	{
        set = true;
	}

    private void Update()
    {
        transform.Rotate(Vector3.forward, 20f * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Teleporter")) && set)
        {
            destination.set = false;
            collision.gameObject.transform.SetPositionAndRotation(destination.transform.position, Quaternion.identity);
        }
    }
	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
            if (temp == null)
            {
                temp = StartCoroutine(Cooldown());
            }
        }
	}

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(.5f);
        set = true;
        temp = null;
    }
}
