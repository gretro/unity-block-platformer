using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public MovementController player;
    public BlockCreator playerBlockCreator;
    public float minTimeBetweenActions = 0.25f;

    private float lastActionAt = 0;

    private int enemyLayer;

    public UnityEvent playerDied;

    private void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        var horizontalAxis = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontalAxis) > 0.5f)
        {
            player.Move(horizontalAxis);
        }

        if (Input.GetButtonDown("Jump"))
        {
            player.Jump();
        }

        // "E" button or Mouse 0
        if (Input.GetButtonDown("Fire1") && CanPerformAction())
        {
            playerBlockCreator.CreateOrDestroyBlock();
            lastActionAt = Time.time;
        // "F" button or Mouse 1
        } else if (Input.GetButtonDown("Fire2") && CanPerformAction())
        {
            playerBlockCreator.CreateOrDestroyGroundBlock();
            lastActionAt = Time.time;
        }
    }

    private bool CanPerformAction()
    {
        var delta = Time.time - lastActionAt;
        return delta >= minTimeBetweenActions;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == enemyLayer)
        {
            playerDied.Invoke();
        }
    }
}
