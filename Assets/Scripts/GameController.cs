using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public int Score { get => _score; set => _score = value; }

    [SerializeField] Texture2D cursorTexture;
    private CursorMode _cursorMode = CursorMode.Auto;
    private Vector2 _hotSpot = Vector2.zero;
    private int _score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        Cursor.SetCursor(cursorTexture, _hotSpot, _cursorMode);
        _score = 0;
        SoundController.Instance.MainMusic.Play();
    }
}
