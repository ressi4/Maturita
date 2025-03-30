using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    public GameObject gunPrefab; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform weaponHolder = other.transform.Find("GlockHolder");

            if (weaponHolder != null)
            {
                GameObject gun = Instantiate(gunPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
                
                Destroy(gameObject); 
            }
        }
    }
}
