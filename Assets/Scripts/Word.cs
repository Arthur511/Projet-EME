using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word", menuName = "Create/Dictionary/Word")]
public class Word : ScriptableObject
{
    public string _word;
    public string _element;
}
