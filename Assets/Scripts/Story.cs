using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dialogs of stories
/// </summary>
[CreateAssetMenu(fileName = "Story", menuName = "Create/Story")]
public class Story : ScriptableObject 
{
    public List<Dialog> StoryDialogs;
}
[Serializable]
public class Dialog
{
    public DialogType type;
    [TextArea]public string Text;
    public Color Color;
    
    public Word word;
    public List<Word> wordsToRespond;
    public TextAnimationType animationType;

}

public enum DialogType
{
    Dialog,
    Naming,
    Sentencing,
    AddWord,
    Fusioning
}


