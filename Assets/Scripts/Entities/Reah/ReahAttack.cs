using UnityEngine;
using System.Collections;
public class reahAttack : MonoBehaviour
{
    public GameObject attackHitbox;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackHitbox.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Attack1");
            StartCoroutine(EnableHitbox());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Attack2");
            StartCoroutine(EnableHitbox());
        }
    }

    IEnumerator EnableHitbox()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);
    }
}
