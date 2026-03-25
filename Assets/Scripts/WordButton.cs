using TMPro;
using UnityEngine;

public class WordButton : MonoBehaviour
{
    string _word;
    string _element;

    public void SetInformation(string word, string element)
    {
        _word = word;
        _element = element;
    }

}
