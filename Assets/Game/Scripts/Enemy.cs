using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D _rigid;
    protected Animator _animator;

    [SerializeField] protected float _moveSpeed;

    [Header("Collision Check")]
    [SerializeField] protected Transform _leftFoot;
    [SerializeField] protected Transform _rightFoot;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected float _wallCheckDistance;
    [SerializeField] protected LayerMask _groundLayer;

    protected bool _isGrounded;
    protected bool _isWallDetected;
    protected int _facingDirection = -1;

    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        CheckGround();
        CheckWall();
        Flip();
    }

    protected virtual void FixedUpdate()
    {
        Move();
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
        Gizmos.DrawLine(_leftFoot.position, new Vector2(_leftFoot.position.x, _leftFoot.position.y + _leftFoot.position.y * -_groundCheckDistance));
        Gizmos.DrawLine(_rightFoot.position, new Vector2(_rightFoot.position.x, _rightFoot.position.y + _rightFoot.position.y * -_groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + _facingDirection * _wallCheckDistance, transform.position.y));
    }

    protected virtual void Move()
    {
        _rigid.velocity = Vector2.right * _facingDirection * _moveSpeed;

        if (!_isGrounded || _isWallDetected)
            _facingDirection *= -1;
    }

    private void Flip()
    {
        transform.rotation = _facingDirection == -1 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }
}
