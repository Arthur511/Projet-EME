using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerEntries : MonoBehaviour
{

    public bool IsWaiting => _currentWord != null;
    public bool IsSentencing => _isSentencing;

    [SerializeField] DialogViewManager _dialogViewManager;
    [SerializeField] GameObject _entriesPanel;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Transform _contentScrollView;
    [SerializeField] Transform _contentLibraryView;
    [SerializeField] GameObject _prefabMessage;
    [SerializeField] GameObject _prefabWord;
    [SerializeField] GameObject _removeLastWordButton;

    bool _isSentencing = false;
    List<Word> _currentSentence = new List<Word>();
    List<Word> _expectedSentence;

    Word _currentWord = null;

    public void WaitingForEntry(Word word)
    {
        _currentWord = word;
        _entriesPanel.SetActive(true);
        _inputField.text = "";
        _inputField.interactable = true;
        _inputField.ActivateInputField();
    }

    public void OnEnter(InputAction.CallbackContext context)
    {
        if (!IsWaiting) return;
        if (context.performed)
        {
            if (_inputField.text != null)
            {
                _currentWord._word = _inputField.text;

                _dialogViewManager.CreateNewMessage(_inputField.text, Color.blue);
                GameObject msg = Instantiate(_prefabWord, _contentLibraryView);
                msg.GetComponent<WordButton>().Initialize(_currentWord);
                msg.GetComponentInChildren<TextMeshProUGUI>().text = _currentWord._word;

                _inputField.text = null;
                _inputField.ActivateInputField();
                _currentWord = null;

            }
            _entriesPanel.SetActive(false);
        }
    }
    public void WaitingForSentence(List<Word> wordsExpected) // 
    {
        _isSentencing = true;
        _expectedSentence = wordsExpected;
        _currentSentence.Clear();
        _entriesPanel.SetActive(true);
        _inputField.interactable = false;
        _inputField.text = null;
        _removeLastWordButton.SetActive(true);

    }

    public void AddWordInSentence(Word word)
    {
        if (!IsSentencing) return;
        _currentSentence.Add(word);
        _inputField.text += word._word + " ";

    }

    public void RemoveLastWordInSentence()
    {
        if (!IsSentencing) return;
        _currentSentence.RemoveAt(_currentSentence.Count -1);
        _inputField.text = string.Join(" ", _currentSentence.ConvertAll(x => x._word));
    }

    public void OnValidateSentence(InputAction.CallbackContext context)
    {
        if (!IsSentencing) return;

        if (context.performed)
        {
            bool isValid = CheckSentence(MainGame.Instance.DialogViewManager.CurrentStory.StoryDialogs[MainGame.Instance.DialogViewManager.CurrentDialogIndex].wordsToRespond);

            if (isValid)
            {
                //_dialogViewManager.CreateNewMessage("Bonne réponse", Color.green);
                _isSentencing = false; // On arrête de parler
                _currentSentence.Clear();
                _inputField.text = null;
                _inputField.interactable = false;
                _entriesPanel.SetActive(false);
                _removeLastWordButton.SetActive(false);

                _dialogViewManager.AutomaticNextDialog();
            }
            else
            {
                //_dialogViewManager.CreateNewMessage("Mauvaise réponse", Color.red);
                _currentSentence.Clear();
                _inputField.text = null;
            }

        }
    }



    public bool CheckSentence(List<Word> expected)
    {
        string sentence = _inputField.text.Trim();
        if (sentence.EndsWith("."))
        {
            sentence = sentence.Substring(0, sentence.Length - 1);
        }

        if (_currentSentence.Count > expected.Count)
        {
            _dialogViewManager.CreateNewMessage("Trop d'informations", Color.red);
            return false;
        }
        if (_currentSentence.Count < expected.Count)
        {
            _dialogViewManager.CreateNewMessage("Pas assez d'informations", Color.red);
            return false;
        }

        for (int i = 0; i < expected.Count; i++)
        {
            if (!_currentSentence.Contains(expected[i]))
            {
                _dialogViewManager.CreateNewMessage("Mauvaise information", Color.red);
                return false;
            }
        }
        return true;
    }


}
