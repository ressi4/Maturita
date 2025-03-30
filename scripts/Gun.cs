using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform player;         
    public Vector2 offset;           

    void Update()
    {
        if (player != null)
        {
            transform.position = (Vector2)player.position + offset;
        }
    }
}
