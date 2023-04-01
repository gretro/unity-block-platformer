using Assets.Scripts.Utils;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField]
    private Transform closedDoor;
    [SerializeField]
    private Transform openedDoor;

    [Header("Collisions")]
    [SerializeField]
    private LayerMask playerLayer;

    [Header("Channels")]
    [SerializeField]
    private LevelChannel levelChannel;

    private bool isOpen = false;

    private void OnEnable()
    {
        levelChannel.ExitOpen.EventHandler += OpenExit;
    }

    private void OnDisable()
    {
        levelChannel.ExitOpen.EventHandler -= OpenExit;
    }

    void Start()
    {
        UpdateDoorSprite();
    }

    public void OpenExit()
    {
        isOpen = true;
        UpdateDoorSprite();
    }

    private void UpdateDoorSprite()
    {
        closedDoor.gameObject.SetActive(!isOpen);
        openedDoor.gameObject.SetActive(isOpen);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen)
        {
            return;
        }

        if (ColliderUtils.IsOnLayerMask(collision.gameObject, playerLayer))
        {
            levelChannel.LevelExit.RaiseEvent();
        }
    }
}
