using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Phantom phantom = other.GetComponent<Phantom>();
        if (phantom != null)
        {
            phantom.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
