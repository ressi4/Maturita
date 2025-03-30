using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        WeaponManager wm = collision.GetComponent<WeaponManager>();
        if (wm == null) return;

        Transform holder = null;

        Pistol pistol = GetComponent<Pistol>();
        Shotgun shotgun = GetComponent<Shotgun>();

        if (pistol != null)
        {
            
            if (wm.hasWeaponOfType(WeaponType.Pistol))
            {
                Pistol weapon = wm.GetWeaponOfType(WeaponType.Pistol) as Pistol;
                weapon.RefillAmmo();
                weapon.UpdateAmmoUI();
                Destroy(gameObject);
                return;
            }
            if(wm.currentWeapon == WeaponType.Shotgun){
                Destroy(gameObject);
                return;
            }

            // Jinak vybav novou pistoli
            holder = collision.transform.Find("GlockHolder");
        }
        else if (shotgun != null)
        {
            if (wm.hasWeaponOfType(WeaponType.Shotgun))
            {
                Shotgun weapon = wm.GetWeaponOfType(WeaponType.Shotgun) as Shotgun;
                weapon.RefillAmmo();
                weapon.UpdateAmmoUI();
                Destroy(gameObject);
                return;
            }
            if(wm.currentWeapon == WeaponType.Pistol){
                wm.RemoveEquippedWeaponObject();
                
            }


            holder = collision.transform.Find("ShotgunHolder");
        }

        if (holder != null)
        {
            transform.SetParent(holder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        if (pistol != null)
        {
            pistol.isEquipped = true;
            wm.EquipWeapon(WeaponType.Pistol);
            pistol.UpdateAmmoUI();
            
        }

        if (shotgun != null)
        {
            shotgun.isEquipped = true;
            wm.EquipWeapon(WeaponType.Shotgun);
            shotgun.UpdateAmmoUI();
        }
    }
}
