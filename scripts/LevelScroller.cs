using UnityEngine;

public class LevelScroller : MonoBehaviour
{
    public float scrollSpeed = 5f; // Rychlost posunu scény

    void Update()
    {
        // Posouvání scény doleva
        transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
    }
}
