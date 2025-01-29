using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; 
    private int currentHealth;

    public Image[] hearts;
    public TextMeshProUGUI deathText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        if (deathText != null)
        {
            deathText.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHearts();
            Debug.Log("Hráč ztratil život! Zbývá: " + currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Hráč zemřel!");

        if (deathText != null)
        {
            deathText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void Heal(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHearts();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = (i < currentHealth);
        }
    }
}
