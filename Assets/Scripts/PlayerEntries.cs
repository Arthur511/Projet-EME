using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerEntries : MonoBehaviour
{

    public bool IsWaiting => _currentWord != null;
    //public bool IsSentencing =>  != null;

    [SerializeField] DialogViewManager _dialogViewManager;

    [SerializeField] GameObject _entriesPanel;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Transform _contentScrollView;
    [SerializeField] Transform _contentLibraryView;
    [SerializeField] GameObject _prefabMessage;
    [SerializeField] GameObject _prefabWord;

    Word _currentWord = null;

    public void WaitingForEntry(Word word)
    {
        _currentWord = word;
        _entriesPanel.SetActive(true);
        _inputField.text = "";
        _inputField.ActivateInputField();
    }
    public void WaitingForSentence(Word word)
    {
        
        
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
                msg.GetComponentInChildren<TextMeshProUGUI>().text = _currentWord._word;
                _inputField.text = null;
                _inputField.ActivateInputField();
                _currentWord = null;

            }
        }
    }

    public void AddWordInSentence()
    {

    }

}
