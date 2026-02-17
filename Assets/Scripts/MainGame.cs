using UnityEngine;
using UnityEngine.InputSystem;

public class MainGame : MonoBehaviour
{

    public static MainGame Instance;

    [Header("References")]
    public DialogViewManager DialogViewManager;
    public PlayerEntries PlayerEntries;
    public WordLibrary WordLibrary;

    [SerializeField] LayerMask _questLayer;

    bool _inQuest = false;

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
        if (!_inQuest && Mouse.current.leftButton.wasPressedThisFrame)
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
            DialogViewManager.CurrentStory = hit.collider.gameObject.GetComponent<Quest>().Story;
            DialogViewManager.InitializeCurrentStory();
            _inQuest = true;
        }
    }


}
