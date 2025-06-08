using UnityEngine;
using System.Collections;
public class reahAttack : MonoBehaviour
{
    public GameObject ReahHitbox;
    private Animator animator;

    public AudioClip soundAttackA;
    public AudioClip soundAttackB;

    void Start()
    {
        animator = GetComponent<Animator>();
        ReahHitbox.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.Instance.PlaySFX(soundAttackA);
            animator.SetTrigger("Attack1");
            StartCoroutine(EnableHitbox());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.PlaySFX(soundAttackB);
            animator.SetTrigger("Attack2");
            StartCoroutine(EnableHitbox());
        }
    }

    IEnumerator EnableHitbox()
    {
        ReahHitbox.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        ReahHitbox.SetActive(false);
    }
}
