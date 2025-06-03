using UnityEngine;

public class ReahHitbox : MonoBehaviour
{
    public int damage = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Phantom enemy = other.GetComponent<Phantom>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
