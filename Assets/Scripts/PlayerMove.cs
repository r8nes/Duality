using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    [Header("Move")]
    [SerializeField] private float _speed = 1f;
    private Vector2 movement;
    private float _moveX = 0f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _feetPosition;
    [SerializeField] private LayerMask _groundLayer;

    private float _groundRadiusChecker;
    private bool _isGrounded;
    private bool _isJumping;

    [Header("WallClimbing")]
    [SerializeField] private Transform _wallUpChecker;
    [SerializeField] private Transform _wallDownChecker;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _upDownSpeed;
    [SerializeField] private float _slideSpeed = 0;
    private float _gravityRef;

    private float _wallUpRadiusChecker;
    private float _wallDownRadiusChecker;
    private bool _onWall;

    private bool _isRight = true;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _gravityRef = _rigidBody.gravityScale;

        _groundRadiusChecker = _feetPosition.GetComponentInChildren<CircleCollider2D>().radius;
        _wallUpRadiusChecker = _wallUpChecker.GetComponentInChildren<CircleCollider2D>().radius;
        _wallDownRadiusChecker = _wallDownChecker.GetComponentInChildren<CircleCollider2D>().radius;
    }

    private void Update()
    {
        GrounCheck();
        WallCheck();

        if (_isRight && _moveX < 0)
        {
            Flip();
        }
        else if (!_isRight && _moveX > 0)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(_moveX * _speed, _rigidBody.velocity.y);
        _rigidBody.velocity = movement;

        MoveWall();
    }

    private void WallCheck()
    {
        _onWall = (Physics2D.OverlapCircle(_wallUpChecker.position, _wallUpRadiusChecker, _wallLayer)) && (Physics2D.OverlapCircle(_wallDownChecker.position, _wallDownRadiusChecker, _wallLayer));
    }

    private void GrounCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(_feetPosition.position, _groundRadiusChecker, _groundLayer);
    }

    private void Jump(bool jump)
    {
        _isJumping = jump;

        if (_isGrounded && _isJumping)
        {
            _rigidBody.velocity = Vector3.up * _jumpForce;
        }
    }

    private void Move(float moveX)
    {
        _moveX = moveX;
    }

    private void MoveWall()
    {
        if (_onWall == true && _isGrounded != false)
        {
            movement.y = Input.GetAxisRaw("Vertical");
            if (movement.y == 0)
            {
                _rigidBody.gravityScale = 0;
                _rigidBody.velocity = new Vector2(0, _slideSpeed);
            }
            if (movement.y != 0)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, movement.y * _upDownSpeed);
            }
        }
        else if (_isGrounded == false && _onWall == false)
        {
            _rigidBody.gravityScale = _gravityRef;
        }
    }

    private void Flip()
    {
        _isRight = !_isRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnEnable()
    {
        PlayerInput.OnJump += Jump;
        PlayerInput.OnMove += Move;
    }

    private void OnDisable()
    {
        PlayerInput.OnMove -= Move;
        PlayerInput.OnJump -= Jump;
    }
}
