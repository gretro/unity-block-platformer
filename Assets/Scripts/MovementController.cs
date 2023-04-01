using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour, IDirectional
{
    [Header("Collisions")]
    [SerializeField]
    private Collider2D groundCollider;
    [SerializeField]
    private Collider2D ceilingCollider;
    [SerializeField]
    private LayerMask blocksLayer;

    [Header("Movement")]
    [SerializeField]
    private float movementSpeed = 200f;
    [SerializeField]
    private float jumpForce = 260f;

    [Header("Channels")]
    [SerializeField]
    public BlockChannel blockChannel;

    [HideInInspector]
    public int Direction { get; private set; } = 1;

    private Rigidbody2D _entity;
    private float _horizontalMovement = 0f;
    private bool _isJumping = false;
    private bool _hasAirControl = false;


    private void Start()
    {
        _entity = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_horizontalMovement) > 0.05f && CanMove())
        {
            var newDirection = _horizontalMovement < 0f ? -1 : 1;
            if (newDirection != Direction)
            {
                Flip();
            }
            Direction = newDirection;

            _entity.velocity = new Vector2(_horizontalMovement * movementSpeed * Time.fixedDeltaTime, _entity.velocity.y);
            _horizontalMovement = 0f;
        } else
        {
            _entity.velocity = new Vector2(0, _entity.velocity.y);
        }

        if (_hasAirControl && IsOnGround())
        {
            _hasAirControl = false;
        }

        if (_isJumping)
        {
            if (CanJump())
            {
                _entity.AddForce(Vector2.up * jumpForce);

                _isJumping = false;
                _hasAirControl = true;
            } else
            {
                blockChannel.DamageBlock.RaiseEvent(transform.position, Vector3Int.up);
                _isJumping = false;
            }
        }
    }

    private void Flip()
    {
        var scale = this._entity.transform.localScale;
        this._entity.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    }

    private bool CanMove()
    {
        return _hasAirControl || IsOnGround();
    }

    private bool CanJump()
    {
        return !IsBelowBlock();
    }

    private bool IsOnGround()
    {
        return groundCollider.IsTouchingLayers(blocksLayer) && Mathf.Abs(_entity.velocity.y) < 0.05f;
    }

    private bool IsBelowBlock()
    {
        return ceilingCollider.IsTouchingLayers(blocksLayer);
    }

    public void Move(float movement)
    {
        _horizontalMovement = movement;
    }

    public void Jump()
    {
        if (_isJumping || !IsOnGround())
        {
            return;
        }

        _isJumping = true;
    }
}
