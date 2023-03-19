using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public MovementController player;

    void Update()
    {
        var horizontalAxis = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontalAxis) > 0.5f)
        {
            player.Move(horizontalAxis);
        }

        if (Input.GetButton("Jump"))
        {
            player.Jump();
        }
    }
}
