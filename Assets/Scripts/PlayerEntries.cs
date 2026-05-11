using System.Collections.Generic;
using System.Linq;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerEntries : MonoBehaviour
{

    public bool IsWaiting => _currentWord != null;
    public bool IsSentencing => _isSentencing;
    public bool IsFusioning => _isFusioning;

    [SerializeField] DialogViewManager _dialogViewManager;
    [SerializeField] GameObject _entriesPanel;
    [SerializeField] GameObject _fusionPanel;
    [SerializeField] Button _fusionModeButton;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Transform _contentScrollView;
    [SerializeField] Transform _contentLibraryView;
    [SerializeField] GameObject _prefabMessage;
    [SerializeField] GameObject _prefabWord;
    [SerializeField] GameObject _removeLastWordButton;
    [SerializeField] List<TextMeshProUGUI> _fusionTextPreview;

    [SerializeField] WordRecipeLibrary _recipeLibrary;

    bool _isSentencing = false;
    bool _isFusioning = false;
    List<Word> _currentSentence = new List<Word>();
    List<Word> _expectedSentence;
    List<Word> _fusionWords = new List<Word>();
    int _indexFusionWord = 0;
    List<GameObject> _wordButtons = new List<GameObject>();

    Word _currentWord = null;

    private void VerifyWordInDouble(string element)
    {
        foreach (GameObject word in _wordButtons)
        {
            if (word.GetComponent<WordButton>() != null)
            {
                if (word.GetComponent<WordButton>()._word._element == element)
                {
                    Destroy(word);
                    _wordButtons.Remove(word);
                    return;
                }
            }
        }
    }

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
        if (!IsWaiting || _isFusioning) return;
        if (context.performed)
        {
            if (_inputField.text != null)
            {
                _currentWord._word = _inputField.text;

                _dialogViewManager.CreateNewMessage(_inputField.text, Color.blue, TextAnimationType.Wave);
                GameObject msg = Instantiate(_prefabWord, _contentLibraryView);
                VerifyWordInDouble(_currentWord._element);
                _wordButtons.Add(msg);
                msg.GetComponent<WordButton>().Initialize(_currentWord);
                msg.GetComponentInChildren<TextMeshProUGUI>().text = _currentWord._word;

                _inputField.text = null;
                _inputField.ActivateInputField();
                _currentWord = null;
                _entriesPanel.SetActive(false);

            }
            else
            {
                _dialogViewManager.CreateNewMessage("Entrée invalide, réessayez", Color.red, TextAnimationType.Shake);
            }
        }
    }

    public void AddWordInLibrary(Word element, string wordName)
    {
        element._word = wordName;
        GameObject msg = Instantiate(_prefabWord, _contentLibraryView);
        VerifyWordInDouble(_currentWord._element);
        _wordButtons.Add(msg);
        msg.GetComponent<WordButton>().Initialize(element);
    }


    #region SENTENCE
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
        _currentSentence.RemoveAt(_currentSentence.Count - 1);
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
                _dialogViewManager.CreateNewMessage(_inputField.text, Color.cornflowerBlue, TextAnimationType.Rainbow);
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
            _dialogViewManager.CreateNewMessage("Trop d'informations", Color.red, TextAnimationType.Shake);
            return false;
        }
        if (_currentSentence.Count < expected.Count)
        {
            _dialogViewManager.CreateNewMessage("Pas assez d'informations", Color.red, TextAnimationType.Shake);
            return false;
        }

        for (int i = 0; i < expected.Count; i++)
        {
            if (!_currentSentence.Contains(expected[i]))
            {
                _dialogViewManager.CreateNewMessage("Mauvaise information", Color.red, TextAnimationType.Shake);
                return false;
            }
        }
        return true;
    }
    #endregion

    #region FUSION
    public void WaitingForFusion()
    {
        _isFusioning = true;
        MainGame.Instance.ToolsMethods.MoveUpUISmooth(_fusionPanel);
        if (MainGame.Instance.IsInQuest)
            _fusionModeButton.interactable = false;
        _fusionWords.Clear();

    }
    public void LeaveFusionMode()
    {
        _isFusioning = false;
        MainGame.Instance.ToolsMethods.MoveDownUISmooth(_fusionPanel);
        if (MainGame.Instance.IsInQuest)
            _fusionModeButton.interactable = true;
        _fusionWords.Clear();

    }

    public void SelectWordForFusion(Word word)
    {
        if (!IsFusioning) return;

        if (_fusionWords.Contains(word))
        {
            return;
        }

        _fusionWords.Add(word);
        _fusionTextPreview[_indexFusionWord].text = word._word;
        _indexFusionWord++;

        if (_fusionWords.Count == 2)
        {
            DoFusioning();
            _indexFusionWord = 0;
        }
    }

    public void DoFusioning()
    {
        _isFusioning = false;

        Word wordOne = _fusionWords[0];
        Word wordTwo = _fusionWords[1];

        WordRecipe recipe = _recipeLibrary.CheckRecipe(wordOne, wordTwo);

        if (recipe != null)
        {
            Word newWord = recipe.NewWord;
            newWord._parentA = wordOne;
            newWord._parentB = wordTwo;


            if (MainGame.Instance.ToolsMethods.Vowels.Contains(wordOne._word[wordOne._word.Length - 1]))
            {
                string compressionWord = wordOne._word.Substring(0, wordOne._word.Length - 1);
                newWord._word = compressionWord + wordTwo._word;
            }
            else
            {
                newWord._word = wordOne._word + wordTwo._word;

            }
            _dialogViewManager.CreateNewMessage($"Fusion de {wordOne._word} et {wordTwo._word} pour créer {newWord._word}", Color.magenta, TextAnimationType.Rainbow);
            GameObject msg = Instantiate(_prefabWord, _contentLibraryView);
            VerifyWordInDouble(newWord._element);
            _wordButtons.Add(msg);
            msg.GetComponent<WordButton>().Initialize(newWord);
            msg.GetComponentInChildren<TextMeshProUGUI>().text = newWord._word;
            _fusionWords.Clear();

            LeaveFusionMode();

            if (MainGame.Instance.IsInQuest)
            {
                _dialogViewManager.AutomaticNextDialog();

            }
        }
        else
        {
            _dialogViewManager.CreateNewMessage($"Aucune fusion possible entre {wordOne._word} et {wordTwo._word}", Color.red, TextAnimationType.None);
            _fusionWords.Clear();
            _isFusioning = true;
        }
        _fusionTextPreview[0].text = "Mot 1";
        _fusionTextPreview[1].text = "Mot 2";
    }

    #endregion


}
