using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class PlayerInput : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField]
    private float minTimeBetweenActions = 0.25f;

    [Header("Channels")]
    [SerializeField]
    private BlockChannel blockChannel = default;
    
    private MovementController _player;
    private float _lastActionAt = 0;

    private void Start()
    {
        _player = GetComponent<MovementController>();
    }

    void Update()
    {
        var horizontalAxis = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontalAxis) > 0.5f)
        {
            _player.Move(horizontalAxis);
        }

        if (Input.GetButtonDown("Jump"))
        {
            _player.Jump();
        }

        // "E" button or Mouse 0
        if (Input.GetButtonDown("Fire1") && CanPerformAction())
        {
            blockChannel.CastOrBanishBlock.RaiseEvent(_player.transform.position, _player.Direction * Vector3Int.right, BlockSource.Player1);
            _lastActionAt = Time.time;
        // "F" button or Mouse 1
        } else if (Input.GetButtonDown("Fire2") && CanPerformAction())
        {
            blockChannel.CastOrBanishBlock.RaiseEvent(_player.transform.position, (_player.Direction * Vector3Int.right) + Vector3Int.down, BlockSource.Player1);
            _lastActionAt = Time.time;
        }
    }

    private bool CanPerformAction()
    {
        var delta = Time.time - _lastActionAt;
        return delta >= minTimeBetweenActions;
    }
}
