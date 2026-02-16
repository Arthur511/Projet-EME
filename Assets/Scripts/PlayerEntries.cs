using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerEntries : MonoBehaviour
{


    [SerializeField] DialogViewManager _dialogViewManager;

    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Transform _contentScrollView;
    [SerializeField] GameObject _prefabMessage;

    public void OnEnter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_inputField.text != null)
            {
                _dialogViewManager.CreateNewMessage(_inputField.text, Color.blue);
                _inputField.text = null;
                _inputField.ActivateInputField();
            }
        }
    }

}
