using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeManagement : MonoBehaviour
{
    public CameraScript cameraScript;
    public GameObject rangedPrefab;
    public GameObject meleePrefab;
    public Phantom phantomScript;

    private GameObject currentPlayer;
    private bool isRanged = true;
    private bool canSwitch = true;
    public float switchCooldown = 2f;

    void Start()
    {
        currentPlayer = Instantiate(rangedPrefab, transform.position, Quaternion.identity);

        cameraScript.player = currentPlayer.transform;

        phantomScript.player.position = currentPlayer.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && canSwitch)
        {
            SwitchCharacter();
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

        phantomScript.player = currentPlayer.transform;

        isRanged = !isRanged;

        Invoke(nameof(ResetSwitch), switchCooldown);
    }

    void ResetSwitch()
    {
        canSwitch = true;
    }

}
