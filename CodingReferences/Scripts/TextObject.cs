using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Josh Bonovich 
 * Project: F.E.I.N.T
 * This scriptable object is used to hold both a string and a color for a text box to use
*/
[CreateAssetMenu(fileName = "Dialog", menuName = "Dialog")]
public class TextObject : ScriptableObject
{
    public string text;
    public Color color;
}
