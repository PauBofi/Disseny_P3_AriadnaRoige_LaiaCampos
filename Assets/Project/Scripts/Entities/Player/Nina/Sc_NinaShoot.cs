using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinaShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    //[SerializeField] Manabar manabar;
    //[SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private NinaMana ninaMana;

    public float bulletSpeed = 50f;
    //private int Mana = 30;
    private float timeToDestroy = 5f;

    // Update is called once per frame
    /*void Start()
    {
        manabar.SetMaxMana(Mana);
        manabar.SetMana(Mana);
    }*/

    public AudioClip SoundBullet;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ninaMana.currentMana > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        AudioManager.Instance.PlaySFX(SoundBullet);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shootDirection = (mousePosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;

        Destroy(bullet, timeToDestroy);

        ninaMana.UseMana(1); // Esto actualiza el valor y la barra
    }

    public void Initialize(NinaMana ninaMana)
    {
        this.ninaMana = ninaMana;
    }


}
