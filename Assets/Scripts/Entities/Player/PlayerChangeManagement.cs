using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerChangeManagement : MonoBehaviour
{
    public CameraScript cameraScript;
    public GameObject rangedPrefab;
    public GameObject meleePrefab;
    //public FinalBossBehaviour finalBossBehaviour;

    private GameObject currentPlayer;
    private bool isRanged = true;
    private bool canSwitch = true;
    public float switchCooldown = 2f;
    public Healthbar ScenealHealthBar;
    public Manabar ScenealManaBar;

    public GameObject HUD_Nina;
    public GameObject HUD_Reah;

    public Action<Transform> OnPlayerChanged;

    public AudioClip soundChange;

    //private int minHealth = 0;

    private float deathYLevel = -65f;
    public GameObject RespawnPoint;

    private bool justSpawned = true;
    private float spawnGraceTime = 1f;
    private float spawnTimer;

    void Start()
    {
        currentPlayer = Instantiate(rangedPrefab, transform.position, Quaternion.identity);
        PlayerMovement playerMovement = currentPlayer.GetComponent<PlayerMovement>();
        PlayerHealth health = currentPlayer.GetComponent<PlayerHealth>();
        health.Initialize(ScenealHealthBar);
        playerMovement.Initialize(ScenealManaBar);
        cameraScript.player = currentPlayer.transform;
        

        health.OnDeath += HandlePlayerDeath;
        OnPlayerChanged?.Invoke(currentPlayer.transform);

        Phantom[] phantoms = FindObjectsOfType<Phantom>();
        foreach (Phantom p in phantoms)
        {
            p.player = currentPlayer.transform;
        }

        spawnTimer = spawnGraceTime;
        justSpawned = true;
    }

    void Update()
    {
        if (justSpawned)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                justSpawned = false;
            }
            return;
        }

        PlayerMovement playerMovement = currentPlayer.GetComponent<PlayerMovement>();

        /*if (playerMovement.currentHealth <= minHealth)
        {
            SceneManager.LoadScene("GameOver");
            Destroy(currentPlayer);
        }*/

        if (Input.GetKeyDown(KeyCode.X) && canSwitch)
        {
            SwitchCharacter();
        }

        if (currentPlayer.transform.position.y < deathYLevel)
        {
            currentPlayer.transform.position = RespawnPoint.transform.position;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }


    void SwitchCharacter()
    {
        canSwitch = false;

        Vector3 position = currentPlayer.transform.position;
        Quaternion rotation = currentPlayer.transform.rotation;

        Destroy(currentPlayer);

        if (isRanged)
        {
            currentPlayer = Instantiate(meleePrefab, position, rotation);
            PlayerMovement playerMovement = currentPlayer.GetComponent<PlayerMovement>();
            PlayerHealth health = currentPlayer.GetComponent<PlayerHealth>();
            health.Initialize(ScenealHealthBar);
            AudioManager.Instance.PlaySFX(soundChange);
            HUD_Nina.SetActive(false);
            HUD_Reah.SetActive(true);
        }
        else
        {
            currentPlayer = Instantiate(rangedPrefab, position, rotation);
            PlayerMovement playerMovement = currentPlayer.GetComponent<PlayerMovement>();
            PlayerHealth health = currentPlayer.GetComponent<PlayerHealth>();
            health.Initialize(ScenealHealthBar);
            playerMovement.Initialize(ScenealManaBar);
            AudioManager.Instance.PlaySFX(soundChange);
            HUD_Reah.SetActive(false);
            HUD_Nina.SetActive(true);
        }

        cameraScript.player = currentPlayer.transform;

        Phantom[] phantoms = FindObjectsOfType<Phantom>();
        foreach (Phantom p in phantoms)
        {
            p.player = currentPlayer.transform;
        }

        isRanged = !isRanged;
        Invoke(nameof(ResetSwitch), switchCooldown);

        OnPlayerChanged?.Invoke(currentPlayer.transform);
    }

    void HandlePlayerDeath()
    {
        SceneManager.LoadScene("GameOver");
        Destroy(currentPlayer);
    }
    void ResetSwitch()
    {
        canSwitch = true;
    }

    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }

}
