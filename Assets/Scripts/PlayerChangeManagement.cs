using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeManagement : MonoBehaviour
{
    public CameraScript cameraScript;
    public GameObject rangedPrefab;
    public GameObject meleePrefab;

    private GameObject currentPlayer;
    private bool isRanged = true;
    private bool canSwitch = true;
    public float switchCooldown = 2f;
    public Healthbar ScenealHealthBar;

    private int minHealth = 0;

    void Start()
    {
        currentPlayer = Instantiate(rangedPrefab, transform.position, Quaternion.identity);
        PlayerMovement playerMovement = currentPlayer.GetComponent<PlayerMovement>();
        playerMovement.healthbar = ScenealHealthBar;
        cameraScript.player = currentPlayer.transform;

        Phantom[] phantoms = FindObjectsOfType<Phantom>();
        foreach (Phantom p in phantoms)
        {
            p.player = currentPlayer.transform;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && canSwitch)
        {
            SwitchCharacter();
        }

        PlayerMovement playerMovement = currentPlayer.GetComponent<PlayerMovement>();

        if (playerMovement.currentHealth <= minHealth)
        {
            Destroy(currentPlayer);
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
        }
        else
        {
            currentPlayer = Instantiate(rangedPrefab, position, rotation);
        }


        cameraScript.player = currentPlayer.transform;

        Phantom[] phantoms = FindObjectsOfType<Phantom>();
        foreach (Phantom p in phantoms)
        {
            p.player = currentPlayer.transform;
        }

        isRanged = !isRanged;

        Invoke(nameof(ResetSwitch), switchCooldown);
    }

    void ResetSwitch()
    {
        canSwitch = true;
    }

}
