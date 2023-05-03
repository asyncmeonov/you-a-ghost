using UnityEngine;
using UnityEngine.Events;

public class MobController : MonoBehaviour
{
    //Definitions
    [SerializeField] MobDefinition mobDef;
    [SerializeField] GameObject bulletPrefab;

    //Events
    [SerializeField] UnityEvent _onMobDeath;

    //Private Vars
    private Animator _anim;
    private Vector3 _lastPosition;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private GameObject _player;
    private GameObject _weaponContainer;
    private bool _isAlive;

    public Rigidbody2D Rb { get => _rb; set => _rb = value; }
    public MobDefinition MobDef { get => mobDef; set => mobDef = value; }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("player");
        _weaponContainer = transform.GetChild(0).gameObject;
        _isAlive = true;
        InvokeRepeating("ShootPlayer", mobDef.shootDelay, mobDef.shootFrequency);

    }

    private void Update()
    {
        if (_isAlive)
        {
            mobDef.mobBrain.Think(this);
            _anim.SetBool("isMoving", Vector3.Distance(_lastPosition, transform.position) > 0); // slightly buggy, stops occasionally
            _lastPosition = transform.position;
        }
        else if (ReferenceEquals(PlayerController.Instance.PotentialAnchor,gameObject))
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (_isAlive)
        {
            //Logic for pointing towards player
            Vector3 playerPos = new Vector3(_player.transform.position.x, _player.transform.position.y + 1f); //offset in y so it shoots at the centre
            Transform containerTransform = _weaponContainer.transform;
            containerTransform.rotation = VectorUtils.GetQuaternionRotationTowards(containerTransform, playerPos);
            _sr.flipX = playerPos.x <= transform.position.x; //flip sprite to face player
        }
    }

    public void Die()
    {
        _isAlive = false;
        PlayerController.Instance.PotentialAnchor = gameObject;
        _anim.SetBool("isAlive", false);
        _weaponContainer.SetActive(false);
        CancelInvoke();
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _onMobDeath.Invoke();
    }

    void ShootPlayer()
    {
        Vector3 _projectileOrigin = _weaponContainer.transform.GetChild(0).GetChild(0).position; //nasty-ass way for retrieval of inner-most child
        Instantiate(bulletPrefab, _projectileOrigin, Quaternion.identity);
    }
}
