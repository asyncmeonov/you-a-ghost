using UnityEngine;


//Responsible for the logic governing bullets from the Monster's Wands
public class MonsterBulletController : MonoBehaviour
{
    [SerializeField] ProjectileDefinition projectile;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            //logic to handle hitting a player
            PlayerController.Instance.Injure();
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
