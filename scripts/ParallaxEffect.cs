using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxSpeed = 0.5f; 
    private Transform cam;
    private Vector3 lastCamPosition;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPosition = cam.position;
    }

    void Update()
    {
        Vector3 deltaMovement = cam.position - lastCamPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxSpeed, 0, 0);
        lastCamPosition = cam.position;
    }
}
