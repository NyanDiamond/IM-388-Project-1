using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to hold the wave data and control how the final boss performs
 * This code also triggers the ending sequence once the required variables are met
*/
public class BossBehavior: MonoBehaviour
{
    bool shieldUp = false;
    float maxHealth = 18;
    float health = 18;
    bool lastHit = false;
    [SerializeField] SpawnableEnemy[] wave1;
    [SerializeField] SpawnableEnemy[] wave2;
    [SerializeField] SpawnableEnemy[] wave3;
    [SerializeField] SpawnableEnemy[] wave4;
    [SerializeField] Vector2[] spawnPoints;
    [SerializeField] TurretBehavior[] turrets;
    [SerializeField] GameObject shield, explosions, finalBarrier, healthBar;
    [SerializeField] SpriteRenderer fadeOut;
    [SerializeField] string nextLevel;
    private AudioSource player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        player.Stop();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (health == maxHealth && collision.CompareTag("Player_Attack"))
		{
            healthBar.SetActive(true);

		}
        if (!shieldUp && collision.CompareTag("Player_Attack") && !lastHit)
        {
            health--;
            healthBar.GetComponent<Slider>().value = health / maxHealth;
            if ((health+2) % 4 == 0)
            {
                if (!player.isPlaying)
                    player.Play();
                shieldUp = true;
                shield.SetActive(true);
                SpawnWave();
            }
        }
        else if(lastHit)
        {
            shieldUp = true;
            lastHit = false;
            finalBarrier.SetActive(true);
            player.Stop();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (finalBarrier.activeInHierarchy && collision.gameObject.CompareTag("Teleporter"))
        {
            Destroy(collision.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().restricted = true;
            healthBar.GetComponent<Slider>().value = 0;
            Finish();
        }
        else if (collision.gameObject.CompareTag("Teleporter"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = (rb.position - collision.GetContact(0).point) * rb.velocity.magnitude;
        }
    }

    void SpawnWave()
    {
        switch(health)
        {
            case 14:
                foreach (SpawnableEnemy enemy in wave1)
                {
                    Debug.Log("spawning");
                    SpawnEnemy(enemy);
                }
                break;
            case 10:
                foreach(SpawnableEnemy enemy in wave2)
                {
                    Debug.Log("spawning");
                    SpawnEnemy(enemy);
                }
                break;
            case 6:
                foreach (SpawnableEnemy enemy in wave3)
                {
                    SpawnEnemy(enemy);
                    foreach (TurretBehavior turret in turrets)
					{
                        turret.Activate();
					}
                }
                break;
            case 2:
                foreach (SpawnableEnemy enemy in wave4)
                {
                    SpawnEnemy(enemy);
                }
                break;
        }
    }

    private void SpawnEnemy(SpawnableEnemy enemy)
    {
        GameObject temp = Instantiate(enemy.enemy, spawnPoints[enemy.spawnPos], Quaternion.identity);
        if (enemy.enemy.transform.childCount != 2)
        {
            EnemyCounter.enemies.Add(temp);
            temp.GetComponent<ExperimentalEnemyMovement>().StealthBreak();
        }
        else
        {
            EnemyCounter.enemies.Add(temp.transform.GetChild(0).gameObject);
            temp.GetComponentInChildren<ExperimentalEnemyMovement>().StealthBreak();
        }
        EnemyCounter.upCount();
        return;
    }


    public void EndWave()
    {
        shield.SetActive(false);
        shieldUp = false;
        if(health<=2)
        {
            lastHit = true;
            foreach (TurretBehavior turret in turrets)
            {
                turret.viewDistance = 0;
            }
        }
    }

    void Finish()
    {
        StartCoroutine(Explosions());
        StartCoroutine(Fade());
        Debug.Log("Game Over");
        return;
    }
    
    private IEnumerator Explosions()
	{
        while (true)
		{
            yield return new WaitForSeconds(Random.Range(0.1f, 1));
            Instantiate(explosions, new Vector3(Random.Range(3.5f, 7.5f), Random.Range(-3.5f, -1), 0), Quaternion.identity);
        }
    }

    private IEnumerator Fade()
    {
        while(fadeOut.color.a < 1)
        {
            yield return new WaitForSeconds(.1f);
            fadeOut.color += new Color(0,0,0,.015f);
        }

        yield return new WaitForSeconds(0.5f);
        SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
        sp.PlaySound11();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(nextLevel);
    }
}
