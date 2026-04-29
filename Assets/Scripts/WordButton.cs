using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Word _word;
    public void Initialize(Word word) // Method to initialize the button with a Word object, each button has the element and his signification
    {
        _word = word;
        GetComponentInChildren<TextMeshProUGUI>().text = _word._word;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PanelForWord.Instance.ShowPanel(_word);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PanelForWord.Instance.HidePanel();
    }

    private void OnClick()
    {
        var entries = MainGame.Instance.PlayerEntries;

        if (entries.IsFusioning)
            entries.SelectWordForFusion(_word);
        else if (entries.IsSentencing)
            entries.AddWordInSentence(_word);
    }

}

