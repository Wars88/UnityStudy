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

    [Header("Ground Check")]
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _leftFoot;
    [SerializeField] Transform _rightFoot;
    [SerializeField] bool _isGrounded;
    
    private PlayerInput _playerInput;
    private Rigidbody2D _rigid;
    private Animator _animator;

    private InputAction _moveAction;
    private InputAction _jumpAction;

    private int _facingDirection = 1; // 1 for right, -1 for left
    private bool _canDoubleJump;

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
        Flip();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
    }

    public void Move()
    {
        float moveX = _moveAction.ReadValue<float>();
        _rigid.velocity = new Vector2(moveX * _moveSpeed, _rigid.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _canDoubleJump = true;
        }
        else if (_canDoubleJump)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, 0);
            _rigid.AddForce(Vector2.up * _doubleJumpForce, ForceMode2D.Impulse);
            _canDoubleJump = false;
        }
    }

    private void CheckGround()
    {
        RaycastHit2D leftFootHit = Physics2D.Raycast(_leftFoot.position, Vector2.down, 0.1f, _groundLayer);
        RaycastHit2D rightFootHit = Physics2D.Raycast(_rightFoot.position, Vector2.down, 0.1f, _groundLayer);

        if (leftFootHit || rightFootHit)
            _isGrounded = true;
        else
            _isGrounded = false;
    }

    private void HandleAnimation()
    {
        _animator.SetFloat("velocityX", _rigid.velocity.x);
        _animator.SetFloat("velocityY", _rigid.velocity.y);
        _animator.SetBool("isGrounded", _isGrounded);
    }

    private void Flip()
    {
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
