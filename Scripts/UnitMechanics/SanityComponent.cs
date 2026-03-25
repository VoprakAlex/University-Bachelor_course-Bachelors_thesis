using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class SanityComponent : MonoBehaviour
{
    [SerializeField] private StatsComponent _statsComponent;

    [field: SerializeField] public int CurrentSanity { get; private set; }
    [field: SerializeField] public bool IsMad { get; private set; }

    [SerializeField] public UnityEvent<int> OnSetSanity;
    [SerializeField] public UnityEvent<int> OnDecreaseSanity;
    [SerializeField] public UnityEvent<int> OnIncreaseSanity;

    [SerializeField] public UnityEvent OnMadness;
    [SerializeField] public UnityEvent OnRecover;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void SetSanity(int newSanity)
    {
        newSanity = Mathf.Clamp(newSanity, 0, _statsComponent.MaxSP);
        CurrentSanity = newSanity;
        OnSetSanity?.Invoke(newSanity);
    }

    public void SetToMax()
    {
        IsMad = false;
        SetSanity(_statsComponent.MaxSP);
    }

    public void DecreaseSanity(int amount)
    {
        if (IsMad) return;
        if (amount <= 0) return;

        SetSanity(CurrentSanity - amount);

        if (CurrentSanity == 0)
            Madness();
        else
            OnDecreaseSanity?.Invoke(CurrentSanity);
    }

    public void IncreaseSanity(int amount)
    {
        if (IsMad) return;
        if (amount <= 0) return;
        if (CurrentSanity == _statsComponent.MaxSP) return;

        SetSanity(CurrentSanity + amount);
        OnIncreaseSanity?.Invoke(CurrentSanity);
    }

    public void Recover(int amount)
    {
        if (!IsMad) return;
        if (amount <= 0) return;

        IsMad = false;
        SetSanity(amount);

        OnRecover?.Invoke();
    }

    public void Madness()
    {
        if (IsMad) return;

        IsMad = true;

        OnMadness?.Invoke();
    }
}