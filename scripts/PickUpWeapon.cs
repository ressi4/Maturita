using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    public Transform weaponHolder; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            transform.SetParent(weaponHolder); 
            transform.localPosition = Vector3.zero; 
            transform.localRotation = Quaternion.identity; 
            
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false; 
        }
    }
}