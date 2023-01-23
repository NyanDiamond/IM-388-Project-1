using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
//Josh Bonovich
//Project F.E.I.N.T.
//This script slowly types out an array of dialog with multiple colors to depict multiple people
//If button(s) are pressed it will skip to the next line or go to the next scene if the dialog is complete
public class DialogBehavior : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] float delay;
    [SerializeField] TextObject[] dialog;
    [SerializeField] string nextScene;
    private int current = 0;
    private string[] loggedText;
    private string currentColor;
    private string currentText;
    // Start is called before the first frame update
    //Load the first string and begin typing it out automatically using a corroutine
    void Start()
    {
        loggedText = new string[dialog.Length];
        currentText = dialog[current].text;
        currentColor = "#" + ColorUtility.ToHtmlStringRGBA(dialog[current].color);
        StartCoroutine(Scroll());

    }
	//Increments current to keep track of what dialog line we are dealing with, and if there is more dialog we update the necessary private variables and run the corroutine again
	void Next()
    {
        current++;
        if(current < dialog.Length)
        {
            currentText = dialog[current].text;
            currentColor = "#" + ColorUtility.ToHtmlStringRGBA(dialog[current].color);
            StartCoroutine(Scroll());
        }
    }

    //Will update the text box with all of the dialog options, adding a new line after each "line" of dialog
    void UpdateText()
    {
        textDisplay.text = loggedText[0];
        for (int i = 1; i< loggedText.Length; i++)
        {
            textDisplay.text += "\n\n" + loggedText[i];
        }
    }

    //Will slowly scroll through the current text, calling UpdateText every time it does, and when it is over it will then call Next
    IEnumerator Scroll()
    {
        for(int s = 0; s<currentText.Length+1; s++)
        {
            loggedText[current] = "<color=" + currentColor + ">" + currentText.Substring(0, s) + "</color>";
            UpdateText();
            yield return new WaitForSeconds(delay);
        }
        Next();
    }

    //Using the new Unity Input System this script will allow you to either skip to the next line of the dialog or go to the next scene if the dialog is finished
    public void OnSelect()
    {
        StopAllCoroutines();
        if(current >= dialog.Length)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            loggedText[current] = "<color="+currentColor+">"+currentText+"</color>";
            UpdateText();
            Next();
        }
    }
}
