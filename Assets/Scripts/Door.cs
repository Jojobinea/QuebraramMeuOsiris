using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject _openDoor;
    [SerializeField] private Collider2D _doorCol;

    private void Start()
    {
        EventManager.onKeyCollectedEvent += OpenTheDoor;
    }

    private void OnDestroy()
    {
        EventManager.onKeyCollectedEvent -= OpenTheDoor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("door trigger");
        if(collision.tag == "Player")
        {
            Debug.Log("door trigger player");
            EventManager.OnReachedDoorTrigger();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.OnReachedDoorTrigger();
        }
    }

    private void OpenTheDoor()
    {
        _openDoor.SetActive(true);
    }
}
