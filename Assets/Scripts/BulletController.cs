using UnityEngine;


//Responsible for the logic governing bullets from the Player's Gun
public class BulletController : MonoBehaviour
{

    //consider using a bullet scriptable object for different bullet types

    private float _speed = 10f;

    private Rigidbody2D _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shootDir = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        _rb.velocity = new Vector2(shootDir.x,shootDir.y).normalized * _speed;
        float rot = Mathf.Atan2(rotation.x, rotation.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot); //rot param expects the bullet to be horizontal pointing right. Adjust angles by adding degrees if sprite is different
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "mob")
        {
            //logic to handle hitting a mob

            collision.gameObject.GetComponent<MobController>().Injure();
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
