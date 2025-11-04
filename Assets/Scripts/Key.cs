using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            EventManager.OnKeyCollectedTrigger();

            gameObject.SetActive(false);
        }
    }
}
