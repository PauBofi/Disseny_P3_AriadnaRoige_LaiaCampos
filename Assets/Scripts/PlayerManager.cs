using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform respawnPoint; // Assign in Inspector
    public float deathYLevel = -10f;

    // Start is called before the first frame update
    private void Update()
    {
        // Optional: auto-death if falling
        if (transform.position.y < deathYLevel)
        {
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Debug.Log("PLAYER MORT // Falta pantalla Game Over");
            Respawn();
        }
    }

    void Respawn()
    {
        // Move player to respawn position and reset velocity
        transform.position = respawnPoint.position;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
