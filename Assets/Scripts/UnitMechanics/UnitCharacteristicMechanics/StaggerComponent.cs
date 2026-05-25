using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent), typeof(HealthComponent))]
public class StaggerComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;
    [SerializeField] private HealthComponent _healthComponent;

    [Header("Stagger")]
    [field: SerializeField] public bool IsStaggered { get; private set; }
    [field: SerializeField] public int StaggerAmount { get; private set; }
    [field: SerializeField] public int StaggerThreshold { get; private set; }

    [Header("StaggerThresholdEvents")]
    public UnityEvent<int> OnSetStaggerThreshold;
    public UnityEvent<int> OnDecreaseStaggerThreshold;
    public UnityEvent<int> OnIncreaseStaggerThreshold;

    [Header("StagerEvents")]
    public UnityEvent OnStagger;
    public UnityEvent OnUnstagger;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
        if (_healthComponent == null)
            _healthComponent = GetComponent<HealthComponent>();
    }

    private void OnEnable()
    {
        if (_healthComponent != null)
        {
            _healthComponent.OnDecreaseHP.AddListener(CheckStagger);
            _healthComponent.OnIncreaseHP.AddListener(CheckStagger);
            _healthComponent.OnSetMaxHealth.AddListener(RefreshStaggerThreshold);
        }
        SetStaggerAmount();
    }

    private void OnDisable()
    {
        if (_healthComponent != null)
        {
            _healthComponent.OnDecreaseHP.RemoveListener(CheckStagger);
            _healthComponent.OnIncreaseHP.RemoveListener(CheckStagger);
            _healthComponent.OnSetMaxHealth.RemoveListener(RefreshStaggerThreshold);
        }
    }

    private void RefreshStaggerThreshold(int currentHealth)
    {
        SetStaggerThreshold(currentHealth - StaggerAmount);
    }

    private void SetStaggerAmount()
    {
        StaggerAmount = Mathf.RoundToInt(_statsComponent.MaxHP * _statsComponent.StaggerPercent);
    }

    private void CheckStagger(int currentHealth)
    {
        if (!IsStaggered && currentHealth <= StaggerThreshold)
        {
            Stagger();
        }
    }

    public void SetStaggerThreshold(int newStaggerThreshold)
    {
        StaggerThreshold = Mathf.Max(0, newStaggerThreshold);
        OnSetStaggerThreshold?.Invoke(StaggerThreshold);
    }

    public void DecreaseStaggerThreshold(int amount)
    {
        if (IsStaggered) return;
        if (amount <= 0) return;

        SetStaggerThreshold(StaggerThreshold - amount);
        OnDecreaseStaggerThreshold?.Invoke(StaggerThreshold);
        CheckStagger(_healthComponent.CurrentHealth);
    }

    public void IncreaseStaggerThreshold(int amount)
    {
        if (IsStaggered) return;
        if (amount <= 0) return;

        SetStaggerThreshold(StaggerThreshold + amount);
        OnIncreaseStaggerThreshold?.Invoke(StaggerThreshold);
        CheckStagger(_healthComponent.CurrentHealth);
    }

    public void Stagger()
    {
        if (IsStaggered) return;
        IsStaggered = true;
        OnStagger?.Invoke();
    }

    public void Unstagger()
    {
        if (!IsStaggered) return;
        IsStaggered = false;
        
        RefreshStaggerThreshold(_healthComponent.CurrentHealth);

        OnUnstagger?.Invoke();
    }
}