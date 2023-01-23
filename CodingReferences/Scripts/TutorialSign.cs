using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This code is used have an interactable sign that displays information 
*/
public class TutorialSign : MonoBehaviour
{
    [SerializeField] List<string> text = new List<string>();
    private bool open = false;
    private int textPos;
    [SerializeField] GameObject textBox;
    [SerializeField] TextMeshPro textBoxText;
    private Animator anim;

	private void Start()
	{
        anim = GetComponent<Animator>();
	}
	void OnContinue()
    {
        if(textBox.activeInHierarchy)
        {
            textPos++;
            if(textPos >= text.Count)
            {
                textPos = 0;
                textBox.SetActive(false);
			}
			else
			{
                anim.SetTrigger("Play");
            }
            textBoxText.text = text[textPos];
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            textPos = 0;
            textBoxText.text = text[textPos];
            textBox.SetActive(true);
            anim.SetTrigger("Play");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            textPos = 0;
            textBoxText.text = text[textPos];
            textBox.SetActive(false);
        }
    }
}
