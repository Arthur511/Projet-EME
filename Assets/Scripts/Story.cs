using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Story", menuName = "Create/Story")]
public class Story : ScriptableObject
{

    public List<Dialog> StoryDialogs;

}

[Serializable]
public class Dialog
{
    public string Text;
    public Color Color;
}
