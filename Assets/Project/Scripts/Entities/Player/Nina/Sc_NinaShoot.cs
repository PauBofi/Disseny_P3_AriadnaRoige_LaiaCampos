using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinaShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    [SerializeField] private NinaMana ninaMana;

    public float bulletSpeed = 50f;
    private float timeToDestroy = 5f;

    public AudioClip SoundBullet;

    //Si se presiona el boton y tiene mana, dispara
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ninaMana.currentMana > 0)
        {
            Shoot();
        }
    }

    //Basicamente se instancia la bala y se le asigna una velocidad al rigidBody de una variable publica en direccion hacia donde se ha pulsado con el click
    void Shoot()
    {
        AudioManager.Instance.PlaySFX(SoundBullet);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shootDirection = (mousePosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;

        Destroy(bullet, timeToDestroy);

        ninaMana.UseMana(1); //Esto actualiza el valor y la barra
    }

    public void Initialize(NinaMana ninaMana)
    {
        this.ninaMana = ninaMana;
    }


}
