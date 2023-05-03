using UnityEngine;


//Responsible for the logic governing bullets from the Monster's Wands
public class MonsterBulletController : MonoBehaviour
{
    [SerializeField] ProjectileDefinition projectile;
    private Rigidbody2D _rb;
    private GameObject _player;
    


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("player");

        Vector3 playerPos = new Vector3(_player.transform.position.x, _player.transform.position.y + 1f);
        Vector3 shootDir = playerPos - transform.position;
        Vector3 rotation = transform.position - playerPos;
        _rb.velocity = new Vector2(shootDir.x,shootDir.y).normalized * projectile.speed;
        float rot = Mathf.Atan2(rotation.x, rotation.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot); //rot param expects the bullet to be horizontal pointing right. Adjust angles by adding degrees if sprite is different
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            //logic to handle hitting a player
            DestroyBullet();
        }

        if (collision.gameObject.tag == "obstacle")
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
