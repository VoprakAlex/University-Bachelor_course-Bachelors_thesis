using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimatorController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private HealthComponent healthComponent;

    [Header("Animator Parameters")]
    [SerializeField] private string attackTrigger = "Attack";
    [SerializeField] private string hitTrigger = "Hit";
    [SerializeField] private string deathTrigger = "Death";

    [Header("Death")]
    [SerializeField] private float AnimationDuration = 1.0f;

    private bool isDead;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (healthComponent == null)
            healthComponent = GetComponentInParent<HealthComponent>();
    }

    private void OnEnable()
    {
        if (healthComponent != null)
        {
            healthComponent.OnDeath.AddListener(PlayDeath);
        }
    }

    private void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.OnDeath.RemoveListener(PlayDeath);
        }
    }

    public void PlayAttack()
    {
        if (isDead) return;

        animator.SetTrigger(attackTrigger);
        StartCoroutine(AttackAnimation());
    }

    public void PlayHit()
    {
        if (isDead) return;

        animator.SetTrigger(hitTrigger);
        StartCoroutine(HitAnimation());
    }

    public void PlayDeath()
    {
        if (isDead) return;

        isDead = true;

        animator.SetTrigger(deathTrigger);

        StartCoroutine(DeathAnimation());
    }

    public IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(AnimationDuration);

        gameObject.SetActive(false);
    }

    public IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(AnimationDuration);

    }

    public IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(AnimationDuration);

    }
}