using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;

    private float _rotSpeed = 1500.0f;
    private Transform _containerTransform;
    private SpriteRenderer _sr;
    private Transform _nozzleTransform;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        //We want to move the gun around the parent container so we will consider all transform operations 
        //to be done on the parent
        _containerTransform = transform.parent;
        _nozzleTransform = GameObject.FindGameObjectWithTag("nozzle").transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletPrefab, _nozzleTransform.position, Quaternion.identity);
        }
    }

    void FixedUpdate()
    {
        //Logic for rotating around mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //in-world coordinates for mouse
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        _containerTransform.rotation = VectorUtils.GetQuaternionRotationTowards(_containerTransform, mousePos, _rotSpeed);

        switch (mousePos.x)
        {
            case < 0:
                _sr.flipY = true;
                break;
            case > 0:
                _sr.flipY = false;
                break;
        }

    }
}
