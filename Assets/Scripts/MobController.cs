using UnityEngine;

public class MobController : MonoBehaviour
{
    [SerializeField] MobDefinition mobDef;
    [SerializeField] GameObject bulletPrefab;


    private RuntimeAnimatorController _anim;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private GameObject _player;
    private Vector3 _projectileOrigin;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>().runtimeAnimatorController;
        _anim = mobDef.animController;
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("player");
        _projectileOrigin = transform.GetChild(0).GetChild(0).GetChild(0).position; //nasty-ass way for retrieval of inner-most child


        InvokeRepeating("ShootPlayer", mobDef.shootDelay, mobDef.shootFrequency);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //movement logic TODO

        //Logic for rotating towards player
        Vector3 playerPos = _player.transform.position;;
        Transform containerTransform = transform.GetChild(0).transform;
        float angle = Mathf.Atan2(playerPos.y - containerTransform.position.y, playerPos.x - containerTransform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        containerTransform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1000f * Time.deltaTime);

        //switch (playerPos.x)
        //{
        //    case < 0:
        //        _sr.flipY = true;
        //        break;
        //    case > 0:
        //        _sr.flipY = false;
        //        break;
        //}
    }

    void ShootPlayer()
    {
       Instantiate(bulletPrefab, _projectileOrigin, Quaternion.identity);
    }
}
