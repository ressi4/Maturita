using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f; // Rychlost pohybu
    public float leftLimit = -5f; // Krajní bod vlevo
    public float rightLimit = 5f; // Krajní bod vpravo

    private int direction = 1; // 1 = doprava, -1 = doleva

    void Update()
    {
        // **Zamkneme Scale, aby se NIKDY nemohla měnit**
        transform.localScale = new Vector3(1, 1, 1);

        // Pohyb platformy po X ose
        transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

        // Změna směru, když dojede na krajní bod
        if (transform.position.x >= rightLimit || transform.position.x <= leftLimit)
        {
            direction *= -1;
        }
    }
}