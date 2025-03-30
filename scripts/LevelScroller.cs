using UnityEngine;

public class LevelScroller : MonoBehaviour
{
    public float scrollSpeed = 5f; 

    void Update()
    {
        
        transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
    }
}
