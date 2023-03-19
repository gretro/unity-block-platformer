using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Collider2D GroundCollider;
    public Collider2D CeilingCollider;

    public Rigidbody2D player;

    public float MovementSpeed = 3f;
    public float JumpForce = 260f;

    private float horizontalMovement = 0f;
    private bool isJumping = false;
    private bool hasAirControl = false;

    private int blocksLayer;

    private void Start()
    {
        blocksLayer = LayerMask.GetMask("Blocks");
    }

    private void FixedUpdate()
    {
        if ((horizontalMovement < -0.05f || horizontalMovement > 0.05f) && CanMove())
        {
            player.transform.Translate(horizontalMovement * MovementSpeed * Time.fixedDeltaTime * Vector3.right);
            horizontalMovement = 0f;
        }

        if (hasAirControl && IsOnGround())
        {
            hasAirControl = false;
        }

        if (isJumping)
        {
            player.AddForce(Vector2.up * JumpForce);

            isJumping = false;
            hasAirControl = true;
        }
    }

    private bool CanMove()
    {
        return hasAirControl || IsOnGround();
    }

    private bool IsOnGround()
    {
        return GroundCollider.IsTouchingLayers(blocksLayer) && Mathf.Abs(player.velocity.y) < 0.05f;
    }

    private bool IsBelowBlock()
    {
        return CeilingCollider.IsTouchingLayers(blocksLayer);
    }

    public void Move(float movement)
    {
        horizontalMovement = movement;
    }

    public void Jump()
    {
        if (isJumping || !IsOnGround())
        {
            return;
        }

        isJumping = true;
    }
}
