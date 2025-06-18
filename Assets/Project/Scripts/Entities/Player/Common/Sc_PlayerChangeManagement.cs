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

    private GameObject currentPlayer;
    private bool isRanged = true;
    private bool canSwitch = true;
    public float switchCooldown = 2f;
    public HealthBar ScenealHealthBar;
    public ManaBar ScenealManaBar;

    public GameObject HUD_Nina;
    public GameObject HUD_Reah;

    public Action<Transform> OnPlayerChanged;

    public AudioClip soundChange;

    public float deathYLevel = -65f;
    public GameObject RespawnPoint;

    private bool justSpawned = true;
    private float spawnGraceTime = 1f;
    private float spawnTimer;

    void Start()
    {
        //Se instancia el jugador al inicio. Siempre la Nina
        currentPlayer = Instantiate(rangedPrefab, transform.position, Quaternion.identity);
        PlayerMovement playerMovement = currentPlayer.GetComponent<PlayerMovement>();

        //Se llama a la función del script de PlayerHealth mediante una referencia que pone la barra de vida.
        PlayerHealth health = currentPlayer.GetComponent<PlayerHealth>();
        health.Initialize(ScenealHealthBar);

        //Se llama a la función del script de NinaMana mediante una referencia que pone la barra de mana.
        NinaMana mana = currentPlayer.GetComponent<NinaMana>();
        mana.Initialize(ScenealManaBar);

        NinaShoot shoot = currentPlayer.GetComponent<NinaShoot>();
        if (shoot != null && mana != null)
        {
            shoot.Initialize(mana);
        }

        cameraScript.player = currentPlayer.transform;
        
        //Nos suscribimos a la función HandlePlayerDeath. Sirve porque así se gestiona la muerte desde aquí, pero la condición de muerte
        //(currentHealth<=0) se encuentra en el script de la vida
        health.OnDeath += HandlePlayerDeath;
        OnPlayerChanged?.Invoke(currentPlayer.transform);

        //Cogemos a todos los phantom del mapa en una array y la recorremos asignandole el transform del player a cada uno (porque va cambiando de jugador)
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
        //Se espera un segundo antes de seguir con el update. Esto se hace porque antes verificaba
        //la muerte del jugador antes de tiempo y lo marcaba como muerto
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

        if (Input.GetKeyDown(KeyCode.X) && canSwitch)
        {
            SwitchCharacter();
        }

        //Sistema en el que mediante la referncia guardada del RespawnPoint, se asigna una nueva posición al player si cae a una altura inferior
        //a -65, que se puede cambiar.
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

        //Aquí se gestiona el cambio mediante una variable bool que nos indica en que personaje estabamos.
        //A partir de ahí, hace lo mismo que en el start
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

            NinaMana mana = currentPlayer.GetComponent<NinaMana>();
            mana.Initialize(ScenealManaBar);

            NinaShoot shoot = currentPlayer.GetComponent<NinaShoot>();
            if (shoot != null && mana != null)
            {
                shoot.Initialize(mana);
            }

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
        SceneManager.LoadScene("S_GameOver");
        Destroy(currentPlayer);
    }
    void ResetSwitch()
    {
        canSwitch = true;
    }
}
