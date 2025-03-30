using JetBrains.Annotations;
using UnityEngine;

public enum WeaponType
{
    None,
    Pistol,
    Shotgun
}

public class WeaponManager : MonoBehaviour
{
    public GameObject Weapon;
    public WeaponType currentWeapon = WeaponType.None;

    public Transform glockHolder;
    public Transform shotgunHolder;
    public bool hasWeaponOfType(WeaponType weapon)
    {
        return currentWeapon == weapon;
    }

    public bool HasWeapon()
    {
        return currentWeapon != WeaponType.None;
    }

    public void EquipWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;

    }

    public void UnequipWeapon()
    {
        currentWeapon = WeaponType.None;
    }

    public void RemoveEquippedWeaponObject()
    {
        // Glock
        if (glockHolder != null && glockHolder.childCount > 0)
        {
            foreach (Transform child in glockHolder)
            {
                GameObject.Destroy(child.gameObject);
            }
            Debug.Log("Glock zničen.");
        }

        // Shotgun
        if (shotgunHolder != null && shotgunHolder.childCount > 0)
        {
            foreach (Transform child in shotgunHolder)
            {
                GameObject.Destroy(child.gameObject);
            }
            Debug.Log("Shotgun zničen.");
        }
    }
    public Component GetWeaponOfType(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Pistol)
        {
            if (glockHolder.childCount > 0)
            {
                Debug.Log(glockHolder.GetChild(0));
                return glockHolder.GetChild(0).gameObject.GetComponent<Pistol>();
            }



        }
        if (weaponType == WeaponType.Shotgun)
        {
            if (shotgunHolder.childCount > 0)
            {
                return shotgunHolder.GetChild(0).gameObject.GetComponent<Shotgun>();
            }
        }
        return null;
    }

    public void ResetEquippedWeaponAmmo()
    {
        Component weaponComponent = GetWeaponOfType(currentWeapon);
        if (weaponComponent == null) return;

        if (weaponComponent is Pistol pistol)
        {
            pistol.ResetAmmo();
            pistol.UpdateAmmoUI();
        }
        else if (weaponComponent is Shotgun shotgun)
        {
            shotgun.ResetAmmo();
            shotgun.UpdateAmmoUI();
        }
    }
}
