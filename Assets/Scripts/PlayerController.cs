using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour

{
    [SerializeField] private float _anchorLength = 2f;

    //Components
    private LineRenderer _tether;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    //Movement
    private float _movSpeed = 5f;
    private Vector2 _movDirection;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _tether = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _movDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _tether.SetPosition(0, transform.position);
        _tether.SetPosition(1, transform.parent.position);

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
        Vector2 position = transform.position;
        Vector2 velocity = _movDirection * _movSpeed;
        Vector2 newPosition = position + velocity * Time.fixedDeltaTime;
        _rb.MovePosition(Vector2.ClampMagnitude(newPosition, _anchorLength));
    }
}
