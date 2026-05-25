using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class ShieldComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Sheild")]
    [field: SerializeField] public int CurrentShield { get; private set; }

    [Header("SheildEvents")]
    public UnityEvent<int> OnSetShield;
    public UnityEvent<int> OnDecreaseShield;
    public UnityEvent<int> OnIncreaseShield;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }


    public void SetShield(int newShield)
    {
        newShield = Mathf.Max(newShield, 0);
        CurrentShield = newShield;
        OnSetShield?.Invoke(newShield);
    }

    public void SetToStarting()
    {
        SetShield(_statsComponent.StartingShield);
    }

    public void DecreaseShield(int amount)
    {
        if (amount <= 0) return;
        int newShield = CurrentShield - amount;
        SetShield(newShield);
        OnDecreaseShield?.Invoke(CurrentShield);
    }

    public void IncreaseShield(int amount)
    {
        if (amount <= 0) return;
        int newShield = CurrentShield + amount;
        SetShield(newShield);
        OnIncreaseShield?.Invoke(CurrentShield);
    }
}