using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordLibrary : MonoBehaviour
{

    [SerializeField] GameObject _prefabWord;    
    
    Dictionary<string, string> _words = new Dictionary<string, string>();

    //[SerializeField] List<Word> _words;


    public void RegisterWord(Word word)
    {
        _words[word._word] = word._element;
    }


    /*public void CreateWord(string nameWord, int indexWord)
    {
        GameObject word = Instantiate(_prefabWord);
        
    }*/
}
