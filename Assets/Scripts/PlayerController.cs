using UnityEngine;
public class PlayerController : MonoBehaviour

{

    public static PlayerController Instance { get; private set; }
    [SerializeField] private float _tetherLength = 2f;

    //Components
    private LineRenderer _tether;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private GameObject _anchor;
    private GameObject _potentialAnchor;
    private bool _hasJumped;

    //Movement
    private float _movSpeed = 5f;
    private Vector2 _movDirection;

    public GameObject PotentialAnchor { get => _potentialAnchor; set => _potentialAnchor = value; }
    public bool HasJumped { get => _hasJumped; set => _hasJumped = value; }

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
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _tether = GetComponent<LineRenderer>();
        _hasJumped = true;
        var anchor = GameObject.FindWithTag("anchor");
        SetAnchor(ref anchor);
    }

    void Update()
    {
        _movDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _tether.SetPosition(0, transform.position);
        _tether.SetPosition(1, GetAnchor().transform.position);

        switch (_movDirection.x)
        {
            case < 0:
                _sr.flipX = true;
                break;
            case > 0:
                _sr.flipX = false;
                break;
        }
    }

    public GameObject GetAnchor()
    {
        return _anchor;
    }
    public void SetAnchor(ref GameObject anchor)
    {
        _anchor = anchor;
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position; //implicitly casting 3D as 2D Vector by reassigning
        Vector2 velocity = _movDirection * _movSpeed;
        Vector2 newPosition = position + velocity * Time.fixedDeltaTime;

        if (PotentialAnchor != null && Input.GetKeyDown("space") && !_hasJumped)
        {
            var potAnchor = PotentialAnchor;
            SetAnchor(ref potAnchor);
            potAnchor.GetComponent<MobController>().IsAnchor = false;
            transform.position = GetAnchor().transform.position;
            _hasJumped = true;
        }

        _rb.MovePosition(VectorUtils.ClampMagnituteAroundPoint(newPosition, GetAnchor().transform.position, _tetherLength));
    }
}
