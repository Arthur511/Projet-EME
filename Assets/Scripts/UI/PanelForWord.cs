using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanelForWord : MonoBehaviour
{
    public static PanelForWord Instance { get; private set; }

    [SerializeField] private GameObject _panelForWord;
    [SerializeField] private TextMeshProUGUI _wordText;
    [SerializeField] private TextMeshProUGUI _meaningText;
    [SerializeField] private RectTransform _rectTransform;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            _panelForWord.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.position = Mouse.current.position.ReadValue() + new Vector2(150, 80); ;
    }
    
    public void ShowPanel(Word word)
    {
        _wordText.text = word._word;
        _meaningText.text = word._element;
        _panelForWord.SetActive(true);
    }

    public void HidePanel() => _panelForWord.SetActive(false);

}
