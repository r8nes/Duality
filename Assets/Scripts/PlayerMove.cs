using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    private float _gravityRef;
    private Rigidbody2D _rigidBody;

    [Header("Move")]
    [SerializeField] private float _speed = 1f;
    private Vector2 movement;
    private float _moveX = 0f;

    // UNDONE
    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _feetPosition;
    [SerializeField] private LayerMask _groundLayer;

    private float _groundRadiusChecker;
    private bool _isGrounded;
    private bool _isJumping;

    // UNDONE
    [Header("WallClimbing")]
    [SerializeField] private Transform _wallChecker;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _upDownSpeed;
    [SerializeField] private float _slideSpeed = 0;

    private bool _onWall;
    private float _wallRadiusChecker;


    // UNDONE
    [Header("WallJump")]
    [SerializeField] private float _jumpWallTime;
    [SerializeField] private float _timerJumpWall;
    [SerializeField] private Vector2 _jumpAngle = new Vector2(3.5f, 10);
    private bool _blockMoveX;

    private bool _isRight = true;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _gravityRef = _rigidBody.gravityScale;

        _groundRadiusChecker = _feetPosition.GetComponentInChildren<CircleCollider2D>().radius;
        _wallRadiusChecker = _wallChecker.GetComponentInChildren<CircleCollider2D>().radius;
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
        WallMove();
        Move();
        WallJump();
    }

    private void WallCheck()
    {
        _onWall = Physics2D.OverlapCircle(_wallChecker.position, _wallRadiusChecker, _wallLayer);
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

    private void Move()
    {
        if (!_blockMoveX)
        {
            _rigidBody.velocity = new Vector2(_moveX * _speed, _rigidBody.velocity.y);
        }
    }

    private void MoveSetter(float moveX)
    {
        _moveX = moveX;
    }

    private void WallMove()
    {
        if (_onWall == true && _isGrounded == false)
        {
            movement.y = Input.GetAxisRaw("Vertical");

            if (!_blockMoveX)
            {
                if (movement.y == 0)
                {
                    _rigidBody.gravityScale = 0;
                    _rigidBody.velocity = new Vector2(0, _slideSpeed);
                }
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

    private void WallJump()
    {
        if (_onWall == true && _isGrounded == false && Input.GetKeyDown(KeyCode.Space))
        {
            _blockMoveX = true;

            Flip();
            _rigidBody.velocity = new Vector2(transform.localScale.x * _jumpAngle.x, _jumpAngle.y);
        }

        if (_blockMoveX && (_timerJumpWall += Time.deltaTime) >= _jumpWallTime)
        {
            if (_onWall == true || _isGrounded == true || _moveX != 0)
            {
                _blockMoveX = false;
                _timerJumpWall = 0;
            }
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
        PlayerInput.OnMove += MoveSetter;
    }

    private void OnDisable()
    {
        PlayerInput.OnMove -= MoveSetter;
        PlayerInput.OnJump -= Jump;
    }
}
