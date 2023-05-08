using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class MobController : MonoBehaviour
{
    //Definitions
    [SerializeField] MobDefinition mobDef;
    [SerializeField] GameObject bulletPrefab;

    //GameObjects and Mono properties
    private Animator _anim;
    private Vector3 _lastPosition;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private GameObject _player;
    private GameObject _weaponContainer;


    //Private Vars
    private bool _isAlive;
    private bool _isAnchor;
    private int _currentHealth;

    public Rigidbody2D Rb { get => _rb; set => _rb = value; }
    public MobDefinition MobDef { get => mobDef; set => mobDef = value; }
    public bool IsAnchor { get => _isAnchor; set => _isAnchor = value; }

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
        _isAnchor = false;
        _currentHealth = MobDef.maxHealth;
        transform.localScale = new Vector2(mobDef.size, mobDef.size);
        InvokeRepeating("ShootPlayer"+mobDef.projectile.attack.ToString(), mobDef.shootDelay, mobDef.shootFrequency); //Change string for different bullet patterns

    }

    private void Update()
    {
        if (_isAlive)
        {
            mobDef.mobBrain.Think(this);
            _anim.SetBool("isMoving", Vector3.Distance(_lastPosition, transform.position) > 0); // slightly buggy, stops occasionally
            _lastPosition = transform.position;
        }
        else if (!_isAlive && IsAnchor)
        {
            StartCoroutine(PulseLight());
        }
        else if (!_isAnchor && PlayerController.Instance.GetAnchor() != gameObject)
        {
            StartCoroutine(Fade(Destroy));
        }
    }

    void FixedUpdate()
    {
        if (_isAlive)
        {
            //Logic for pointing towards player
            Vector3 playerPos = new Vector3(_player.transform.position.x, _player.transform.position.y);
            Transform containerTransform = _weaponContainer.transform;
            containerTransform.rotation = VectorUtils.GetQuaternionRotationTowards(containerTransform, playerPos);
            _sr.flipX = playerPos.x <= transform.position.x; //flip sprite to face player
        }
    }

    public void Injure()
    {
        _currentHealth -= 1;
        if(_currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Shake(0.1f,0.2f));
        }
    }

    public void Die()
    {
        SoundController.Instance.MobDeath.Play();
        GetComponent<Collider2D>().enabled = false;
        GameController.Instance.Score += MobDef.reward;
        PlayerController.Instance.HasJumped = false;
        GameObject previosAnchor = PlayerController.Instance.PotentialAnchor;
        if (previosAnchor != null)
        {
            previosAnchor.GetComponent<MobController>().IsAnchor = false;
        }
        _isAnchor = true;
        _isAlive = false;
        PlayerController.Instance.PotentialAnchor = gameObject;
        _anim.SetBool("isAlive", false);
        _weaponContainer.SetActive(false);
        CancelInvoke();
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private IEnumerator Fade(Action lastAction = null)
    {
        Color c = _sr.material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            _sr.material.color = c;
            yield return null;
        }
        lastAction.Invoke();
    }

    private IEnumerator PulseLight()
    {
        Light2D light = GetComponent<Light2D>();
        light.enabled = true;
        Color A = Color.cyan;
        Color B = Color.white;
        Color lerpedColor = Color.Lerp(A, B, Mathf.PingPong(Time.time, 1));
        light.intensity = Mathf.Lerp(0,1,Mathf.PingPong(Time.time,1));
        _sr.material.color = lerpedColor;
        yield return null;
    }

    private IEnumerator Shake(float duration = 1f, float magnitude = 1f)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.position = originalPos + Random.insideUnitSphere * magnitude;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    void ShootPlayerLinearAim()
    {
        Vector3 _projectileOrigin = _weaponContainer.transform.GetChild(0).GetChild(0).position; //nasty-ass way for retrieval of inner-most child
        for (int i = 0; i < mobDef.projectile.count; i++)
        {
            var proj = Instantiate(bulletPrefab, _projectileOrigin, Quaternion.identity);

        Rigidbody2D _projRb = proj.GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("player");

        Vector3 playerPos = new Vector3(_player.transform.position.x, _player.transform.position.y);
        Vector3 shootDir = playerPos - transform.position;
        Vector3 rotation = transform.position - playerPos;
        _projRb.velocity = new Vector2(shootDir.x, shootDir.y).normalized * MobDef.projectile.speed;
        float rot = Mathf.Atan2(rotation.x, rotation.y) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, rot); //rot param expects the bullet to be horizontal pointing right. Adjust angles by adding degrees if sprite is different
        proj.transform.localScale = new Vector2(mobDef.projectile.size, mobDef.projectile.size);
        }
    }

    void ShootPlayerRadial()
    {
        Vector2 _projectileOrigin = _weaponContainer.transform.GetChild(0).GetChild(0).position; //nasty-ass way for retrieval of inner-most child
        int _numberOfProjectiles = MobDef.projectile.count;
        float _radius = 5f;
        float angleStep = 360f / _numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {

            float projectileDirXposition = _projectileOrigin.x + Mathf.Sin((angle * Mathf.PI) / 180) * _radius;
            float projectileDirYposition = _projectileOrigin.y + Mathf.Cos((angle * Mathf.PI) / 180) * _radius;

            Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
            Vector2 projectileMoveDirection = (projectileVector - _projectileOrigin).normalized * MobDef.projectile.speed;

            var proj = Instantiate(bulletPrefab, _projectileOrigin, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity =
                new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
            proj.transform.localScale = new Vector2(mobDef.projectile.size, mobDef.projectile.size);

            angle += angleStep;
        }
    }

    void ShootPlayerRandom()
    {
        Vector3 _projectileOrigin = _weaponContainer.transform.GetChild(0).GetChild(0).position; //nasty-ass way for retrieval of inner-most child
        var proj = Instantiate(bulletPrefab, _projectileOrigin, Quaternion.identity);
        Rigidbody2D _projRb = proj.GetComponent<Rigidbody2D>();
        Vector3 randomDir = new Vector3(Random.value, Random.value, 0).normalized;
        Vector3 shootDir = randomDir - transform.position;
        Vector3 rotation = transform.position - randomDir;
        _projRb.velocity = new Vector2(shootDir.x, shootDir.y).normalized * MobDef.projectile.speed;
        float rot = Mathf.Atan2(rotation.x, rotation.y) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, rot); //rot param expects the bullet to be horizontal pointing right. Adjust angles by adding degrees if sprite is different
        proj.transform.localScale = new Vector2(mobDef.projectile.size, mobDef.projectile.size);
    }
}
