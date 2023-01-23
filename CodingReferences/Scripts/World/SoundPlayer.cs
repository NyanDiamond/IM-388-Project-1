using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used to hold and use a set list of audio clips
 * Origionally this was going to be used with Animation Events, which at the time I wasn't sure could take a variable...hence why they are seperated like this and not just one method that takes 
 * an integer parameter. Though at one point it was used directly in the code and by the time it was in so many places I felt changing it would have done more work than what it would have saved
*/
public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip[] clips = new AudioClip[15];
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound1()
    {
        audioSource.PlayOneShot(clips[0]);
    }

    public void PlaySound2()
    {
        audioSource.PlayOneShot(clips[1]);
    }

    public void PlaySound3()
    {
        audioSource.PlayOneShot(clips[2]);
    }

    public void PlaySound4()
    {
        audioSource.PlayOneShot(clips[3]);
    }

    public void PlaySound5()
    {
        audioSource.PlayOneShot(clips[4]);
    }

    public void PlaySound6()
    {
        audioSource.PlayOneShot(clips[5]);
    }

    public void PlaySound7()
    {
        audioSource.PlayOneShot(clips[6]);
    }

    public void PlaySound8()
    {
        audioSource.PlayOneShot(clips[7]);
    }

    public void PlaySound9()
    {
        audioSource.PlayOneShot(clips[8]);
    }

    public void PlaySound10()
    {
        audioSource.PlayOneShot(clips[9]);
    }

    public void PlaySound11()
    {
        audioSource.PlayOneShot(clips[10]);
    }

    public void PlaySound12()
    {
        audioSource.PlayOneShot(clips[11]);
    }

    public void PlaySound13()
    {
        audioSource.PlayOneShot(clips[12]);
    }

    public void PlaySound14()
    {
        audioSource.PlayOneShot(clips[13], .5f);
    }
}
