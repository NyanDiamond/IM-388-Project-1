using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Josh Bonovich & Ian Connors
 * Project: F.E.I.N.T
 * This code is used to contol the total health of the player as well as produce a loss state when the player loses all of their hp
*/
public class PlayerHealthBehavior : MonoBehaviour
{
    public Color normal;
    public Color transparent;
    [SerializeField] private int health = 3;
    [Tooltip("How many flashes the sprite makes to signify invulnerable (each flash is .4 seconds")]
    public int invulnerability;
    private PlayerMovement pm;
    private SpriteRenderer sr;
    private bool invulnerable = false;
    //private Text healthBox;
    private GameObject deathMenu;

    private GameObject healthbox;
    private List<Image> healthVisuals;
    [SerializeField] Sprite damaged;
    [SerializeField] Sprite undamaged;
    private void Start()
    {
        healthVisuals = new List<Image>();
        healthbox = GameObject.FindGameObjectWithTag("HealthInfo");
        foreach (Transform child in healthbox.transform)
        {
            healthVisuals.Add(child.gameObject.GetComponent<Image>());
        }
        for (int i = 0; i < healthVisuals.Count; i++)
        {
            if (i < health)
                healthVisuals[i].sprite = undamaged;
            else
                healthVisuals[i].sprite = damaged;
        }
        deathMenu = GameObject.FindGameObjectWithTag("DeathBox");
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
        //healthBox.text = "Health: 3";
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy_Attack") && !invulnerable)
        {
            //Destroy(collision.gameObject);
            invulnerable = true;
            health--;
            healthVisuals[health].sprite = damaged;
            //healthBox.text = "Health: " + health;
            if (health > 0)
            {
                pm.Damaged();
                StartCoroutine(InvulnerabilityCD());
                SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
                sp.PlaySound7();
            }
            else if (health<=0)
            {
                pm.Death();
                SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
                sp.PlaySound8();
                StartCoroutine(WaitDeath());
            }
        }
    }
    public void RecoverHealth()
	{
        if (health < 3)
		{
            healthVisuals[health].sprite = undamaged;
            health++;
        }
	}
    IEnumerator InvulnerabilityCD()
    {
        int temp = 0;

        while(temp<invulnerability)
        {
            yield return new WaitForSeconds(0.2f);
            sr.color = transparent;
            yield return new WaitForSeconds(0.2f);
            sr.color = normal;
            temp++;
        }
        invulnerable = false;
    }
    IEnumerator WaitDeath()
	{
        yield return new WaitForSeconds(2f);
        deathMenu.GetComponent<PauseMenu>().OnPause();
    }
}
