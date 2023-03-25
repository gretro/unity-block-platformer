using UnityEngine;
using UnityEngine.Events;

public class LevelExit : MonoBehaviour
{
    public Transform Closed;
    public Transform Opened;

    public UnityEvent LevelExited;

    private Collider2D doorTrigger;
    private int playerLayer;
    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        doorTrigger = GetComponent<Collider2D>();
        playerLayer = LayerMask.NameToLayer("Player");

        UpdateDoorSprite();
    }

    public void OpenExit()
    {
        isOpen = true;
        UpdateDoorSprite();
    }

    private void UpdateDoorSprite()
    {
        Closed.gameObject.SetActive(!isOpen);
        Opened.gameObject.SetActive(isOpen);

        doorTrigger.enabled = isOpen;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            Debug.Log("Level completed");
            LevelExited.Invoke();
        }
    }
}
