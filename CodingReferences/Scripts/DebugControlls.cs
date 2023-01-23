using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DebugControlls : MonoBehaviour
{

    public GameObject enemy;
    // Start is called before the first frame update
    void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void OnSpawn()
    {
        Camera cam = GameObject.FindObjectOfType<Camera>();
        Vector2 pos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Instantiate(enemy, pos, Quaternion.identity);
    }
    void OnStealthBreak()
    {
        EnemyCounter.StealthBreak();
    }
    void OnTeleporterReturn()
	{
        GameObject teleporter = GameObject.FindGameObjectWithTag("Teleporter");
        if (teleporter != null)
		{
            if (teleporter.GetComponent<TeleporterBehavior>().attached == false)
			{
                Destroy(teleporter);
			}
		}
	}
}
