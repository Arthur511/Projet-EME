using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordLibrary : MonoBehaviour
{

    Dictionary<string, string> _words = new Dictionary<string, string>();

    public void RegisterWord(string word, string element)
    {
        _words[word] = element;
    }
}
