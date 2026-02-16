using UnityEngine;
using UnityEngine.InputSystem;

public class MainGame : MonoBehaviour
{

    public static MainGame Instance;


    [Header("References")]
    [SerializeField] DialogViewManager _dialogViewManager;
    [SerializeField] PlayerEntries _playerEntries;

    [SerializeField] LayerMask _questLayer;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckQuest();
        }

    }

    private void CheckQuest()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (1 << hit.collider.gameObject.layer == _questLayer.value)
        {
            _dialogViewManager.CurrentStory = hit.collider.gameObject.GetComponent<Quest>().Story;
            _dialogViewManager.InitializeCurrentStory();
        }
    }


}
