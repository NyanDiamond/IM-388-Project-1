using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/*
 * Ian Connors
 * Project: F.E.I.N.T
 * This code is used to contol all the UI elements that are connected to the stealth mechanics and door locking mechanics
*/
public class EnemyInfoController : MonoBehaviour
{
	public Text exitInfo;
	public Text exitInfo1;
	public Text guardInfo;
	public Text turretInfo;
	public Text guardCountInfo;
	public Text timer;
	public Text checkpointInfo;
	public Image lockIcon;
	public Sprite lockedSprite;
	public Sprite unlockedSprite;
	public Image alarmFill;
	public GameObject screenFilm;
	public Light2D mainLight;
	public Text areaNumber;
	private bool stealthBroken = false;
	private bool timerEndEarly = false;
	private void Start()
	{
		StealthStart();
		if (PlayerPrefs.GetInt("NewCheckpoint") == 1)
		{
			CheckpointNotice(PlayerPrefs.GetInt("checkpoint"));
		}
	}
	public void StealthBreak()
	{
		mainLight.pointLightOuterRadius = 10;
		screenFilm.SetActive(false);

		stealthBroken = true;
		StartCoroutine(Alarm());
		if (EnemyCounter.enemies.Count > 0 || EnemyCounter.turrets.Count > 0)
		{
			exitInfo.gameObject.SetActive(true);
			StartCoroutine(SlowDisappear(exitInfo));
		}
		else
		{
			guardCountInfo.gameObject.SetActive(true);
			StartCoroutine(GuardCount());
			exitInfo1.gameObject.SetActive(true);
			StartCoroutine(SlowDisappear(exitInfo1));
		}

		lockIcon.sprite = lockedSprite;
		lockIcon.color = new Color(lockIcon.color.r, lockIcon.color.g, lockIcon.color.b, 1);

		if (EnemyCounter.enemies.Count > 0)
		{

			//enemyInfoText.text = "! [YOU'VE BEEN SPOTTED] !";
			//enemyInfoText.color = Color.red;
			guardInfo.gameObject.SetActive(true);
			StartCoroutine(SlowDisappear(guardInfo));
			
			guardCountInfo.gameObject.SetActive(true);
			StartCoroutine(GuardCount());
		}
		if (EnemyCounter.turrets.Count > 0)
		{
			turretInfo.gameObject.SetActive(true);
			StartCoroutine(SlowDisappear(turretInfo));
		}
		
			//enemyInfoText.text = "! [YOU'VE BEEEN SPOTTED] !      -Turrets are activated but no one is here to lock the door-";
			//enemyInfoText.color = Color.red;
		/*else
        {
			//enemyInfoText.text = "! [YOU'VE BEEEN SPOTTED] !      -but no one is here to lock the door, escape to the right-";
			//enemyInfoText.color = Color.green;
		}*/
		
	}
	public void EnterTimedRoom(int time)
	{
		lockIcon.color = new Color(0, 0, 0, 1);

		timer.gameObject.SetActive(true);
		timer.text = (time).ToString();
		StartCoroutine(TimerCountdown(time));

		exitInfo.gameObject.SetActive(true);
		StartCoroutine(SlowDisappear(exitInfo));
	}
	public void AllEnemiesDead()
	{
		mainLight.pointLightOuterRadius = 10;
		screenFilm.SetActive(false);
		
		exitInfo1.gameObject.SetActive(true);
		StartCoroutine(SlowDisappear(exitInfo1));
		stealthBroken = false;

		lockIcon.sprite = unlockedSprite;
		lockIcon.color = new Color(1, 1, 1, 0.2f);
		//enemyInfoText.text = "! [ALL ENEMIES DEAD] !     -escape through the door on the right-";
		//enemyInfoText.color = Color.green;
	}
	public void CheckpointNotice(int checkpointNumber)
	{
		checkpointInfo.gameObject.SetActive(true);
		StartCoroutine(SlowDisappear(checkpointInfo));
		areaNumber.text = "AREA 0" + checkpointNumber;
	}
	public void StealthStart()
	{
		stealthBroken = false;
		screenFilm.SetActive(true);

		lockIcon.sprite = unlockedSprite;
		lockIcon.color = new Color(1, 1, 1, 0.2f);
		//enemyInfoText.text = "[Stealth Mode]";
		//enemyInfoText.color = Color.yellow;
	}
	public void EnterDarkRoom()
	{
		mainLight.pointLightOuterRadius = 0;
		StealthStart();
		//enemyInfoText.text = "[Stealth Mode]";
		//enemyInfoText.color = Color.yellow;
	}
	public void EnterBrightRoom()
	{
		mainLight.pointLightOuterRadius = 6;
		StealthStart();
	}
	public void EndTimerEarly()
	{
		timerEndEarly = true;
		timer.text = "0";
	}
	private IEnumerator TimerCountdown(int t)
	{
		for (int i = t; i > 0; i--)
		{
			if (!timerEndEarly)
			{
				timer.text = i.ToString();
				yield return new WaitForSeconds(1f);
			}
			else
			{
				timer.text = "0";
			}
		}
		timer.gameObject.SetActive(false);

		lockIcon.sprite = unlockedSprite;
		lockIcon.color = new Color(1, 1, 1, 0.2f);

		exitInfo1.gameObject.SetActive(true);
		StartCoroutine(SlowDisappear(exitInfo1));
		timerEndEarly = false;
		
	}
	private IEnumerator GuardCount()
	{
		guardCountInfo.color = Color.yellow;

		if (EnemyCounter.count != 1)
			guardCountInfo.text = EnemyCounter.count + " guards left";
		else
			guardCountInfo.text = "1 guard left";

		while (stealthBroken && EnemyCounter.count > 0)
		{
			yield return new WaitUntil(() => EnemyCounter.countChanged);

			if (EnemyCounter.count != 1)
				guardCountInfo.text = EnemyCounter.count + " guards left";
			else
				guardCountInfo.text = "1 guard left";
		}

		StartCoroutine(SlowDisappear(guardCountInfo));
	}
	private IEnumerator SlowDisappear(Text textObj)
	{
		textObj.color = new Color(textObj.color.r, textObj.color.g, textObj.color.b, 1);

		yield return new WaitForSeconds(1f + -100 / textObj.gameObject.transform.localPosition.y);
		for (int i = 0; i < 10; i++)
		{
			yield return new WaitForSeconds(0.1f);
			textObj.transform.Translate(Vector3.down);
			textObj.color -= new Color(0, 0, 0, 0.1f);
		}
		textObj.gameObject.SetActive(false);
		textObj.transform.Translate(Vector3.up * 10);
	}
	private IEnumerator Alarm()
	{
		Debug.Log("Alarm");
		while (stealthBroken)
		{
			for (int i = 0; i < 10; i++)
			{
				yield return new WaitForSeconds(0.05f);
				alarmFill.color += new Color(0, 0, 0, 0.1f);
			}
			yield return new WaitForSeconds(0.1f);
			for (int i = 0; i < 10; i++)
			{
				yield return new WaitForSeconds(0.05f);
				alarmFill.color -= new Color(0, 0, 0, 0.1f);
			}
		}
		alarmFill.color = new Color(1, 1, 1, 0f);
	}
}
