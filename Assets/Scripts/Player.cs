using System;
using UnityEngine;
using static Utils;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _baseSpeed = 1f;
    [SerializeField] private float _baseJumpForce = 1f;
    private float _speed;
    private float _jumpForce;
    private bool isGrounded = false;

    private bool _isJump = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _speed = _baseSpeed * 0.12f;
        _jumpForce = _baseJumpForce * 2.4f;
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
		{
            _isJump = false;
		}	
		
        Move();
    }

    private void FixedUpdate()
    {
        if (CheckKeyDown(KeyCode.Space))
        {
            _isJump = true;
        }

        if (_isJump && isGrounded)
        {
            Jump();
            Debug.Log("Jump!");
        }
    }

    private void Move()
    {
		var input = Input.GetAxis("Horizontal");
        var offset = new Vector3(_speed * input, 0, 0);
        var booferVelocity = _rigidbody.velocity;

        if (input == 0)
		{
            booferVelocity.x = 0;
		}
        else
        {
            booferVelocity.x = _speed * input;
        }
        _rigidbody.velocity = booferVelocity;
    }

    private void Jump()
    {
        var offset = new Vector2(0, _jumpForce * 2);
        
        var booferVelocity = _rigidbody.velocity;
        booferVelocity.y = 0;
        _rigidbody.velocity = booferVelocity;

        _rigidbody.AddRelativeForce(offset);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ground" || collision.transform.tag == "RingEdge")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "RingEdge")
        {
            isGrounded = false;
        }
    }
}
