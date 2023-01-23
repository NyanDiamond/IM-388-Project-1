using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ian Connors
 * Project: F.E.I.N.T
 * This code is used simply play a sound on creation and destroy itself when the explosion animation ends
*/
public class GeneratorExplosion : MonoBehaviour
{
    private void Start()
    {
        GameObject.FindObjectOfType<SoundPlayer>().PlaySound14();
    }
    private void AnimationDone()
	{
		Destroy(gameObject);
	}
}
