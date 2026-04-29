using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class DialogViewManager : MonoBehaviour
{
    public Story CurrentStory { get; set; }
    public int CurrentDialogIndex => _currentStoryIndex;

    [SerializeField] GameObject _prefabMessage;
    [SerializeField] Transform _contentScrollView;

    int _currentStoryIndex = 0;
    List<GameObject> _messages = new List<GameObject>();

    public void CreateNewMessage(string text, Color color)
    {
        GameObject msg = Instantiate(_prefabMessage, _contentScrollView);
        _messages.Add(msg);
        msg.GetComponent<TextMeshProUGUI>().text = text;
        msg.GetComponent<TextMeshProUGUI>().color = color;
    }

    public void InitializeCurrentStory()
    {
        _currentStoryIndex = 0;
        foreach (GameObject msg in _messages)
        {
            Destroy(msg);
        }
        _messages.Clear();
        CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);
    }

    public void NextDialog(InputAction.CallbackContext context)
    {
        if (MainGame.Instance.PlayerEntries.IsWaiting) return;
        if (context.performed)
        {
            Debug.Log(_currentStoryIndex);
            if (_currentStoryIndex < CurrentStory.StoryDialogs.Count - 1)
            {
                _currentStoryIndex++;
                if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Dialog)
                    CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);
                else if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Naming)
                {
                    CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);
                    MainGame.Instance.PlayerEntries.WaitingForEntry(CurrentStory.StoryDialogs[_currentStoryIndex].word);
                }
                else if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Sentencing)
                {
                    CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);
                    MainGame.Instance.PlayerEntries.WaitingForSentence(CurrentStory.StoryDialogs[_currentStoryIndex].wordsToRespond);
                }
            }
        }
    }

    public void AutomaticNextDialog()
    {
        _currentStoryIndex++;
        if (_currentStoryIndex < CurrentStory.StoryDialogs.Count)
        {
            if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Dialog)
                CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);
            else if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Naming)
            {
                MainGame.Instance.PlayerEntries.WaitingForEntry(CurrentStory.StoryDialogs[_currentStoryIndex].word);
            }
            else if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Sentencing)
            {
                MainGame.Instance.PlayerEntries.WaitingForSentence(CurrentStory.StoryDialogs[_currentStoryIndex].wordsToRespond);
            }
        }
    }


}
