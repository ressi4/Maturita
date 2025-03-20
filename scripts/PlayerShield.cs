using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public bool shieldActive = false;
    private bool canUseShield = false;
    public float shieldCooldown = 2f;
    public GameObject shieldVisual;

    void Start()
    {
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }
    }

  

    public void ActivateShield()
    {
        shieldActive = true;
        canUseShield = false;

        if (shieldVisual != null)
        {
            shieldVisual.SetActive(true);
        }

        Debug.Log("Štít aktivován!");
    }

    public void BreakShield()
    {
        if (shieldActive)
        {
            shieldActive = false;

            if (shieldVisual != null)
            {
                shieldVisual.SetActive(false);
            }

            Debug.Log("Štít zničen!");
            StartCoroutine(ShieldCooldown());
        }
    }

    IEnumerator ShieldCooldown()
    {
        yield return new WaitForSeconds(shieldCooldown);
        canUseShield = true;
    }

    public bool IsShieldActive()
    {
        return shieldActive;
    }
}
