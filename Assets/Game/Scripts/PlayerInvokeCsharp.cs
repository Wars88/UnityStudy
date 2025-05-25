using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInvokeCsharp : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpForce = 5f;
    [SerializeField] float _doubleJumpForce = 5f;

    [Header("Collision Check")]
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _leftFoot;
    [SerializeField] Transform _rightFoot;
    [SerializeField] float _groundCheckDistance;
    [SerializeField] float _wallCheckDistance;

    [Header("Wall Interaction")]
    [SerializeField] float _wallJumpDuration = 0.6f;
    [SerializeField] Vector2 _wallJumpForce;
    [SerializeField] bool _wallJumping;

    [Header("KnockBack Interaction")]
    [SerializeField] float _knockBackDuration = 1f;
    [SerializeField] Vector2 _knockBackForce;
    [SerializeField] bool _isKnockBack;
    [SerializeField] bool _canKnockBack;

    private PlayerInput _playerInput;
    private Rigidbody2D _rigid;
    private Animator _animator;

    private InputAction _moveAction;
    private InputAction _jumpAction;

    private int _facingDirection = 1; // 1 for right, -1 for left
    private bool _isGrounded;
    private bool _canDoubleJump;
    private bool _isWallDetected;
    private bool _isDoubleJumping;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
    }

    private void OnEnable()
    {
        _playerInput.actions.Enable();

        _jumpAction.performed += Jump;
    }

    private void OnDisable()
    {
        _playerInput.actions.Disable();

        _jumpAction.performed -= Jump;
    }

    private void Update()
    {
        if (_isKnockBack)
            return;

        Flip();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        if (_isKnockBack)
            return;

        CheckGround();
        HandleLand();
        CheckWall();
        Move();
        WallSlide();
    }

    public void Move()
    {
        if (_wallJumping)
            return;

        float moveX = _moveAction.ReadValue<float>();
        _rigid.velocity = new Vector2(moveX * _moveSpeed, _rigid.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_isKnockBack)
            return;

        if (_isGrounded)
        {
            _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _canDoubleJump = true;
        }
        else if (_isWallDetected && _rigid.velocity.y < 0)
        {
            _rigid.velocity = new Vector2(_wallJumpForce.x * -_facingDirection, _wallJumpForce.y);
            Flip();
            StopAllCoroutines();
            StartCoroutine(WallJumpRoutine());
            _canDoubleJump = true;
        }
        else if (_canDoubleJump)
        {
            _isDoubleJumping = true;
            _rigid.velocity = new Vector2(_rigid.velocity.x, 0);
            _rigid.AddForce(Vector2.up * _doubleJumpForce, ForceMode2D.Impulse);
            _canDoubleJump = false;
        }
    }

    private void WallSlide()
    {
        if (_isWallDetected && _rigid.velocity.y < 0)
        {
            _isDoubleJumping = false;
            _rigid.velocity = new Vector2(_rigid.velocity.x, _rigid.velocity.y * 0.5f);
        }
    }

    public void KnockBack()
    {
        _animator.SetTrigger("knockBack");

        StartCoroutine(KnockBackRoutine());

        _rigid.velocity = new Vector2(_knockBackForce.x * -_facingDirection, _knockBackForce.y);
    }

    private IEnumerator WallJumpRoutine()
    {
        _wallJumping = true;

        yield return new WaitForSeconds(_wallJumpDuration);

        _wallJumping = false;
    }

    private IEnumerator KnockBackRoutine()
    {
        _canKnockBack = false;
        _isKnockBack = true;

        yield return new WaitForSeconds(_knockBackDuration);

        _isKnockBack = false;
        _canKnockBack = true;
    }

    private void CheckGround()
    {
        RaycastHit2D leftFootHit = Physics2D.Raycast(_leftFoot.position, Vector2.down, _groundCheckDistance, _groundLayer);
        RaycastHit2D rightFootHit = Physics2D.Raycast(_rightFoot.position, Vector2.down, _groundCheckDistance, _groundLayer);

        if (leftFootHit || rightFootHit)
            _isGrounded = true;
        else
            _isGrounded = false;
    }

    private void CheckWall()
    {
        _isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * _facingDirection, _wallCheckDistance, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_leftFoot.position, new Vector2(_leftFoot.position.x, _leftFoot.position.y  + _leftFoot.position.y * -_groundCheckDistance));
        Gizmos.DrawLine(_rightFoot.position, new Vector2(_rightFoot.position.x, _rightFoot.position.y + _rightFoot.position.y * -_groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + _facingDirection * _wallCheckDistance, transform.position.y));
    }

    private void HandleLand()
    {
        if (_isGrounded)
        {
            _canDoubleJump = true;
            _isDoubleJumping = false;
        }
    }

    private void HandleAnimation()
    {
        _animator.SetFloat("velocityX", _rigid.velocity.x);
        _animator.SetFloat("velocityY", _rigid.velocity.y);
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetBool("isWallDetected", _isWallDetected);
        _animator.SetBool("isDoubleJumping", _isDoubleJumping);
    }

    private void Flip()
    {
        if (_wallJumping)
        {
            if (_rigid.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                _facingDirection = 1;
            }
            else if (_rigid.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                _facingDirection = -1;
            }

            return;
        }

        float moveX = _moveAction.ReadValue<float>();

        if (moveX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _facingDirection = 1;
        }
        else if (moveX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _facingDirection = -1;
        }
    }
}
