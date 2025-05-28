using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    //[SerializeField] Manabar manabar;
    [SerializeField] private PlayerMovement playerMovement;

    public float bulletSpeed = 50f;
    //private int Mana = 30;
    private float timeToDestroy = 5f;

    // Update is called once per frame
    /*void Start()
    {
        manabar.SetMaxMana(Mana);
        manabar.SetMana(Mana);
    }*/

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerMovement.currentMana > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shootDirection = (mousePosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;

        Destroy(bullet, timeToDestroy);

        playerMovement.UseMana(1); // Esto actualiza el valor y la barra
    }

}
