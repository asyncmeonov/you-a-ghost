using UnityEngine;
public class PlayerController : MonoBehaviour

{
    [SerializeField] private float _tetherLength = 2f;

    //Components
    private LineRenderer _tether;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private GameObject _anchor;
    private GameObject _potentialAnchor;

    //Movement
    private float _movSpeed = 5f;
    private Vector2 _movDirection;

    public GameObject PotentialAnchor { get => _potentialAnchor; set => _potentialAnchor = value; }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _tether = GetComponent<LineRenderer>();
        _anchor = GameObject.FindWithTag("anchor");

        var test = new Vector2(4, 4);
        Debug.Log(test);
        Debug.Log(Vector2.ClampMagnitude(test,2));
    }

    // Update is called once per frame
    void Update()
    {
        _movDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        _tether.SetPosition(0, transform.position);
        _tether.SetPosition(1, _anchor.transform.position);

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

    private void FixedUpdate()
    {
       

        Vector2 position = transform.position; //implicitly casting 3D as 2D Vector by reassigning
        Vector2 velocity = _movDirection * _movSpeed;
        Vector2 newPosition = position + velocity * Time.fixedDeltaTime;


        if (PotentialAnchor != null && Input.GetKeyDown("space"))
        {
            _anchor = PotentialAnchor;
            transform.position = _anchor.transform.position;
        }

        _rb.MovePosition(VectorUtils.ClampMagnituteAroundPoint(newPosition, _anchor.transform.position, _tetherLength));
    }
}
