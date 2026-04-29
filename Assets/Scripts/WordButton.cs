using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordButton : MonoBehaviour
{
    Word _word;
    
    public void Initialize(Word word) // Method to initialize the button with a Word object, each button has the element and his signification
    {
        _word = word;
        GetComponentInChildren<TextMeshProUGUI>().text = _word._word;

        GetComponent<Button>().onClick.AddListener(() => 
        {
            Debug.Log($"Button clicked: {_word._word} - {_word._element}");
            MainGame.Instance.PlayerEntries.AddWordInSentence(_word);
        });
    }
}
