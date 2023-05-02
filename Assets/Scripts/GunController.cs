using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunController : MonoBehaviour
{

    private float _rotSpeed = 1500.0f;
    private Transform _containerTransform;

    private SpriteRenderer _sr;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        //We want to move the gun around the parent container so we will consider all transform operations 
        //to be done on the parent
        _containerTransform = transform.parent;
    }

    void FixedUpdate()
    {
        //Logic for rotating around mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //in-world coordinates for mouse
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        float angle = Mathf.Atan2(mousePos.y - _containerTransform.position.y, mousePos.x - _containerTransform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _containerTransform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);

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
