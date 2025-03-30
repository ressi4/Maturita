using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public bool shieldActive = false;
    private bool canUseShield = false;
    public float shieldCooldown = 2f;
    public GameObject shieldVisual;

    public AudioClip shieldBreakSound;
    private AudioSource audioSource;

    void Start()
    {
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
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

            
            if (shieldBreakSound != null && audioSource != null)
            {
                StartCoroutine(PlayShieldBreakSoundLimited());
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

    
    IEnumerator PlayShieldBreakSoundLimited()
    {
        audioSource.clip = shieldBreakSound;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        audioSource.Stop();
    }
}
