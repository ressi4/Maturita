using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public float leftLimit = -5f;
    public float rightLimit = 5f; 

    private int direction = 1; 

    void Update()
    {
        
        transform.localScale = new Vector3(1, 1, 1);

        
        transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

        
        if (transform.position.x >= rightLimit || transform.position.x <= leftLimit)
        {
            direction *= -1;
        }
    }
}