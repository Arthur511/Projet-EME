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

    [SerializeField] Story _currentStory;
    [SerializeField] GameObject _prefabMessage;
    [SerializeField] Transform _contentScrollView;

    int _currentStoryIndex = 0;
    List<GameObject> _messages = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InitializeCurrentStory();
    }

    // Update is called once per frame
    void Update()
    {

    }

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
        CreateNewMessage(_currentStory.StoryDialogs[_currentStoryIndex].Text, _currentStory.StoryDialogs[_currentStoryIndex].Color);
    }

    public void NextDialog(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(_currentStoryIndex);
            if (_currentStoryIndex < _currentStory.StoryDialogs.Count-1)
            {
                _currentStoryIndex++;
                CreateNewMessage(_currentStory.StoryDialogs[_currentStoryIndex].Text, _currentStory.StoryDialogs[_currentStoryIndex].Color);
            }
        }
    }


}
