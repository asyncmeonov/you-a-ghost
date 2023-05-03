using UnityEngine;

public class MobController : MonoBehaviour
{
    [SerializeField] MobDefinition mobDef;
    [SerializeField] GameObject bulletPrefab;

    private Vector2 _movDirection;
    private RuntimeAnimatorController _anim;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private GameObject _player;
    private Vector3 _projectileOrigin;

    public Rigidbody2D Rb { get => _rb; set => _rb = value; }
    public MobDefinition MobDef { get => mobDef; set => mobDef = value; }

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>().runtimeAnimatorController;
        _anim = mobDef.animController;
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("player");
        InvokeRepeating("ShootPlayer", mobDef.shootDelay, mobDef.shootFrequency);

    }

    private void Update()
    {
        mobDef.mobBrain.Think(this);
    }

    void FixedUpdate()
    {
        //Logic for pointing towards player
        Vector3 playerPos = new Vector3(_player.transform.position.x, _player.transform.position.y + 1f); //offset in y so it shoots at the centre
        Transform containerTransform = transform.GetChild(0).transform;
        containerTransform.rotation = VectorUtils.GetQuaternionRotationTowards(containerTransform, playerPos);

        _sr.flipX = playerPos.x <= transform.position.x; //flip sprite to face player
    }

    void ShootPlayer()
    {
        _projectileOrigin = transform.GetChild(0).GetChild(0).GetChild(0).position; //nasty-ass way for retrieval of inner-most child
        Instantiate(bulletPrefab, _projectileOrigin, Quaternion.identity);
    }
}
