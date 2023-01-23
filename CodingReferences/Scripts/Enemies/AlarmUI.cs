using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to contol the alarm coloring and disapearing in terms of the stealth mechanics
*/
public class AlarmUI : MonoBehaviour
{
    public Color safe;
    public Color warning;
    public Color spotted;
    public Color alarmRaised;
    private GameObject alarm;
    private SpriteRenderer sr;
    
    //public Text alarmText;

    
    private void Start()
    {
        alarm = transform.Find("Alarm").gameObject;
        sr = alarm.GetComponent<SpriteRenderer>();
        sr.color = safe;
        //alarmText.text = "Safe";
    }

    public void Safe()
    {
        if(sr!=null)
            sr.color = safe;
        //alarmText.text = "Safe";
    }

    public void Warning()
    {
        if (sr != null)
            sr.color = warning;
        //alarmText.text = "Warning";
    }

    public void Spotted()
    {
        if (sr != null)
            sr.color = spotted;
        //alarmText.text = "Spotted";
    }

    public void AlarmRaised()
    {
        if (sr != null)
            sr.color = alarmRaised;

        SoundPlayer sp = GameObject.Find("GameController").GetComponent<SoundPlayer>();
        sp.PlaySound1();
        //alarmText.text = "Alarm Raised!";
    }

    private void OnDestroy()
    {
        Destroy(alarm);
    }
}
