using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        // Kamera se hýbe jen na X ose, ale Z a Y zůstávají stejné
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, -15f);
    }
}
