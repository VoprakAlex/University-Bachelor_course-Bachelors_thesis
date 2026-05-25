using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class HealthComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Health")]
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public bool IsDead { get; private set; }

    [Header("HealthEvents")]
    public UnityEvent<int> OnSetHealth;
    public UnityEvent<int> OnSetMaxHealth;
    public UnityEvent<int> OnDecreaseHP;
    public UnityEvent<int> OnIncreaseHP;

    [Header("DeathEvents")]
    public UnityEvent OnDeath;
    public UnityEvent OnRevive;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void SetHealth(int newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, _statsComponent.MaxHP);
        CurrentHealth = newHealth;
        OnSetHealth?.Invoke(newHealth);
    }

    public void SetToMax()
    {
        IsDead = false;
        SetHealth(_statsComponent.MaxHP);
        OnSetMaxHealth?.Invoke(_statsComponent.MaxHP);
    }

    public void DecreaseHP(int amount)
    {
        if (IsDead) return;
        if (amount <= 0) return;

        SetHealth(CurrentHealth - amount);

        if (CurrentHealth == 0)
            Death();
        else
            OnDecreaseHP?.Invoke(CurrentHealth);
    }

    public void IncreaseHP(int amount)
    {
        if (IsDead) return;
        if (amount <= 0) return;
        if (CurrentHealth == _statsComponent.MaxHP) return;

        SetHealth(CurrentHealth + amount);
        OnIncreaseHP?.Invoke(CurrentHealth);
    }

    public void Revive(int amount)
    {
        if (!IsDead) return;
        if (amount <= 0) return;

        IsDead = false;
        SetHealth(amount);

        OnRevive?.Invoke();
    }

    public void Death()
    {
        if (IsDead) return;

        IsDead = true;

        OnDeath?.Invoke();
    }
}