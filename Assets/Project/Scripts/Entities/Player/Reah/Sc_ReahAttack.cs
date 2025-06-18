using UnityEngine;
using System.Collections;
public class ReahAttack : MonoBehaviour
{
    public GameObject ReahHitbox;
    private Animator animator;

    public AudioClip soundAttackA;
    public AudioClip soundAttackB;

    public string reahFirstAttackKey = "Q";
    public string reahSecondAttackKey = "E";

    private KeyCode firstKey;
    private KeyCode secondKey;

    private float animationTime = 0.7f;
    void Start()
    {
        animator = GetComponent<Animator>();
        ReahHitbox.SetActive(false);

        firstKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), reahFirstAttackKey);
        secondKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), reahSecondAttackKey);
    }

    //Verifica si se pulsa la Q o la E para hacer el ataque. Ademas se activa la hitbox de otro script para que esta gestione el daño
    void Update()
    {
        if (Input.GetKeyDown(firstKey))
        {
            AudioManager.Instance.PlaySFX(soundAttackA);
            animator.SetTrigger("Attack1");
            StartCoroutine(EnableHitbox());
        }
        else if (Input.GetKeyDown(secondKey))
        {
            AudioManager.Instance.PlaySFX(soundAttackB);
            animator.SetTrigger("Attack2");
            StartCoroutine(EnableHitbox());
        }
    }

    //Activa la hitbox durante 0.7 segundos (tiempo de animacion)
    IEnumerator EnableHitbox()
    {
        ReahHitbox.SetActive(true);
        yield return new WaitForSeconds(animationTime);
        ReahHitbox.SetActive(false);
    }
}
