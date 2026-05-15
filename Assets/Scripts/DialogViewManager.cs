using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] float _scrollDuration = 0.5f;

    int _currentStoryIndex = 0;
    Dialog _currentDialog;
    List<GameObject> _messages = new List<GameObject>();


    private void Start()
    {
        _currentDialog = CurrentStory.StoryDialogs[_currentStoryIndex];
    }

    public void CreateNewMessage(string text, Color color, TextAnimationType textAnimation)
    {
        List<GameObject> messagesToMove = _messages.ToList();
        GameObject temp = Instantiate(_prefabMessage, _contentScrollView);
        temp.GetComponent<TextMeshProUGUI>().text = text;

        float contentWidth = (_contentScrollView as RectTransform).rect.width;
        RectTransform rect = temp.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(contentWidth, 0f);

        LayoutRebuilder.ForceRebuildLayoutImmediate(temp.GetComponent<RectTransform>());
        float msgHeight = temp.GetComponent<TextMeshProUGUI>().GetPreferredValues(text, contentWidth, 0f).y;
        Debug.Log("Message height: " + msgHeight);
        Destroy(temp);
        StartCoroutine(MoveMessages(messagesToMove, msgHeight, text, color, textAnimation));
    }

    public void InitializeCurrentStory()
    {
        _currentStoryIndex = 0;
        foreach (GameObject msg in _messages)
        {
            Destroy(msg);
        }
        _messages.Clear();
        CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color, CurrentStory.StoryDialogs[_currentStoryIndex].animationType);
    }

    public void NextDialog(InputAction.CallbackContext context)
    {
        if (MainGame.Instance.PlayerEntries.IsWaiting || MainGame.Instance.PlayerEntries.IsSentencing || MainGame.Instance.PlayerEntries.IsFusioning) return;
        if (context.performed)
        {
            if (_currentStoryIndex < CurrentStory.StoryDialogs.Count - 1)
            {
                _currentStoryIndex++;
                if (CurrentStory.StoryDialogs[_currentStoryIndex].Text != null)
                    CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color, CurrentStory.StoryDialogs[_currentStoryIndex].animationType);
                /*if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Dialog)
                    CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);*/
                if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Naming)
                {
                    //CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);
                    MainGame.Instance.PlayerEntries.WaitingForEntry(CurrentStory.StoryDialogs[_currentStoryIndex].word);
                }
                else if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Sentencing)
                {
                    //CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);
                    MainGame.Instance.PlayerEntries.WaitingForSentence(CurrentStory.StoryDialogs[_currentStoryIndex].wordsToRespond);
                }
                else if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.AddWord)
                {
                    //CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);
                    MainGame.Instance.PlayerEntries.AddWordInLibrary(CurrentStory.StoryDialogs[_currentStoryIndex].word, CurrentStory.StoryDialogs[_currentStoryIndex].Text);
                }
                else if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Fusioning)
                {
                    MainGame.Instance.PlayerEntries.WaitingForFusion();
                }
            }
            else
            {
                CreateNewMessage("Quest's end", Color.white, CurrentStory.StoryDialogs[_currentStoryIndex].animationType = TextAnimationType.None);
                MainGame.Instance.IsInQuest = false;
                MainGame.Instance.CurrentQuest.RevealNextQuests();
                MainGame.Instance.PlayerEntries.FusionModeButton.interactable = true;
            }
        }
    }

    public void AutomaticNextDialog()
    {
        _currentStoryIndex++;
        if (_currentStoryIndex < CurrentStory.StoryDialogs.Count)
        {
            /*if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Dialog)
                CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color);*/
            CreateNewMessage(CurrentStory.StoryDialogs[_currentStoryIndex].Text, CurrentStory.StoryDialogs[_currentStoryIndex].Color, CurrentStory.StoryDialogs[_currentStoryIndex].animationType);
            if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Naming)
            {
                MainGame.Instance.PlayerEntries.WaitingForEntry(CurrentStory.StoryDialogs[_currentStoryIndex].word);
            }
            else if (CurrentStory.StoryDialogs[_currentStoryIndex].type == DialogType.Sentencing)
            {
                MainGame.Instance.PlayerEntries.WaitingForSentence(CurrentStory.StoryDialogs[_currentStoryIndex].wordsToRespond);
            }

        }
    }

    IEnumerator MoveMessages(List<GameObject> messagesToMove, float heightToReach, string text, Color color, TextAnimationType textAnimation)
    {

        float elapsedTime = 0;
        Vector2[] startPositions = messagesToMove.Select(x => x.GetComponent<RectTransform>().anchoredPosition).ToArray();
        Vector2[] endPositions = startPositions.Select(x => x + new Vector2(0, heightToReach)).ToArray();
        while (elapsedTime < _scrollDuration)
        {
            for (int i = 0; i < messagesToMove.Count; i++)
            {
                if (messagesToMove[i] != null)
                {
                    messagesToMove[i].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPositions[i], endPositions[i], elapsedTime / _scrollDuration);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < messagesToMove.Count; i++)
        {
            if (messagesToMove[i] != null)
            {
                messagesToMove[i].GetComponent<RectTransform>().anchoredPosition = endPositions[i];
            }
        }

        InstanciateMessage(text, color, textAnimation);
    }

    private void InstanciateMessage(string text, Color color, TextAnimationType textAnimation)
    {
        GameObject msg = Instantiate(_prefabMessage, _contentScrollView);
        msg.GetComponentInChildren<TextMeshProUGUI>().text = text;
        msg.GetComponentInChildren<TextMeshProUGUI>().color = color;

        _messages.Add(msg);

        if (textAnimation != TextAnimationType.None)
        {
            var animator = msg.AddComponent<TextAnimator>();
            animator.StartAnimation(textAnimation, 3f);
        }
    }

}
