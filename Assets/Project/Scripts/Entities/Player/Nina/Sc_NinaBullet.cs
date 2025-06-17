using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinaBullet : MonoBehaviour
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

        FinalBossBehaviour FinalBossBehaviour = other.GetComponent<FinalBossBehaviour>();
        if (FinalBossBehaviour != null)
        {
            FinalBossBehaviour.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
