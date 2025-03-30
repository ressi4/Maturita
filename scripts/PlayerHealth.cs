using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public Image[] hearts;
    public GameObject deathText;
    public GameObject button;
    public GameObject hitEffect;
    public AudioClip hitSound;

    private float edgeStayTime = 0f;
    public float timeToDie = 3f;
    private Camera mainCam;
    private bool isInKillZone = false;
    public GameObject player; // přetáhneš Hráče z Hierarchie
    public Transform playerStartPoint; // přetáhneš Empty objekt, kde hráč startuje
    public Transform cameraStartPoint; // nastavíš v Unity na výchozí pozici kamery
    public GameObject firstPlatform;
    public InfiniteBackground backgroundScript;


    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        if (deathText != null)
        {
            deathText.SetActive(false);
            button.SetActive(false);
        }

        mainCam = Camera.main;
    }

    void Update()
    {
        CheckKillZone();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHearts();
            Debug.Log("Hráč ztratil život! Zbývá: " + currentHealth);

            if (CameraShake.instance != null)
            {
                CameraShake.instance.shakeMagnitude = damage >= 2 ? 0.2f : 0.1f;
                CameraShake.instance.Shake();
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        player.GetComponent<script>().PlayerDied();
        FindObjectOfType<WaveShooter>().StopWaves();

        if (deathText != null)
            deathText.SetActive(true);

        if (button != null)
            button.SetActive(true);
    }

    public void Heal(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHearts();
            Debug.Log("Hráč získal život! Aktuální HP: " + currentHealth);
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = (i < currentHealth);
        }
    }

    void CheckKillZone()
    {
        Vector3 screenPos = mainCam.WorldToViewportPoint(transform.position);

        bool outOfView = screenPos.x < 0f || screenPos.x > 1f || screenPos.y < 0f || screenPos.y > 1f;

        if (outOfView)
        {
            if (!isInKillZone)
            {
                isInKillZone = true;
                edgeStayTime = 0f;
            }

            edgeStayTime += Time.deltaTime;

            if (edgeStayTime >= timeToDie && currentHealth > 0)
            {
                Debug.Log("Hráč byl moc dlouho mimo obraz -> smrt");
                TakeDamage(currentHealth);
            }
        }
        else
        {
            isInKillZone = false;
            edgeStayTime = 0f;
        }
    }

    public void FakeRestart()
    {

        WeaponManager weaponManager = player.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.ResetEquippedWeaponAmmo();
            weaponManager.UnequipWeapon();
            weaponManager.RemoveEquippedWeaponObject();

        }
        var scriptRef = player.GetComponent<script>();
        Camera.main.transform.position = cameraStartPoint.position;
        scriptRef.ResetScore();
        FindObjectOfType<WaveShooter>().StartWaveRoutine();

        
        List<GameObject> foundObjects = new List<GameObject>();
        String[] tagsToDestroy = {"EnemyBullet", "Generated", "EnemyBigBullet"};
        foreach (string tag in tagsToDestroy)
        {
            
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foundObjects.AddRange(objects);
        }
        foreach (GameObject obj in foundObjects)
        {
            Destroy(obj);
        }


        player.transform.position = playerStartPoint.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;


        currentHealth = maxHealth;
        UpdateHearts();


        player.GetComponent<script>().enabled = true;


        if (deathText != null)
        {
            deathText.SetActive(false);

        }

        if (button != null)
        {
            button.SetActive(false);

        }


        if (scriptRef.scoreEndText != null)
            scriptRef.scoreEndText.gameObject.SetActive(false);

        if (scriptRef.newHighScoreText != null)
            scriptRef.newHighScoreText.gameObject.SetActive(false);

        SpawnPrefab spawner = firstPlatform.GetComponent<SpawnPrefab>();
        if (spawner != null)
        {
            spawner.spawnDone = false;

        }
        backgroundScript.ResetBackground();
        player.GetComponent<script>().scoreText.gameObject.SetActive(true);

    }



}
