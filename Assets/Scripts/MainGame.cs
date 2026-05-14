using UnityEngine;
using UnityEngine.InputSystem;

public class MainGame : MonoBehaviour
{

    public static MainGame Instance;

    [Header("References")]
    public DialogViewManager DialogViewManager;
    public PlayerEntries PlayerEntries;
    public ToolsMethods ToolsMethods;

    public bool IsInQuest { get => _inQuest; set => _inQuest = value; }
    public Quest CurrentQuest { get => _currentQuest; set => _currentQuest = value; }

    [SerializeField] LayerMask _questLayer;
    //[SerializeField] LayerMask _wordLayer;
    [SerializeField] GameObject _firstQuest;

    bool _inQuest = false;
    Quest _currentQuest;
    bool _isInFusionWordMode = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToolsMethods.SpawnScaleSmooth(_firstQuest);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_inQuest && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckQuest();
        }
        /*if (_isInFusionWordMode && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckWord();
        }*/

    }

    private void CheckQuest()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (1 << hit.collider.gameObject.layer == _questLayer.value)
        {
            PlayerEntries.FusionModeButton.interactable = false;
            ToolsMethods.BounceScale(hit.collider.gameObject);
            _currentQuest = hit.collider.gameObject.GetComponent<Quest>();
            DialogViewManager.CurrentStory = _currentQuest.Story;
            DialogViewManager.InitializeCurrentStory();
            _inQuest = true;
        }
    }


    #region OBSOLETE
    private void CheckWord()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        /*if (1 << hit.collider.gameObject.layer == _wordLayer.value)
        {
            //hit.collider.gameObject.GetComponent<WordButton>().SetInformation();
        }*/
    }
    #endregion


}
