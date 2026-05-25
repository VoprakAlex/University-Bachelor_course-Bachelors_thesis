using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class SpeedComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Speed")]
    [field: SerializeField] public int CurrentSpeed { get; private set; }

    [Header("SpeedEvents")]
    public UnityEvent<int> OnSetSpeed;
    public UnityEvent<int> OnIncreaseSpeed;
    public UnityEvent<int> OnDecreaseSpeed;

    private void Awake()
    {
        if (_statsComponent == null)
        {
            _statsComponent = GetComponent<StatsComponent>();
        }
    }

    public void SetSpeed(int newSpeed)
    {
        CurrentSpeed = Mathf.Max(newSpeed, 1);
        OnSetSpeed?.Invoke(newSpeed);
    }

    public void SetRandomSpeed()
    {
        int randomSpeed = Random.Range(_statsComponent.MinSpeed, _statsComponent.MaxSpeed);
        SetSpeed(randomSpeed);
    }

    public void IncreaseSpeed(int amount)
    {
        SetSpeed(CurrentSpeed + amount);
        OnIncreaseSpeed?.Invoke(CurrentSpeed);
    }

    public void DecreaseSpeed(int amount)
    {
        SetSpeed(CurrentSpeed - amount);
        OnDecreaseSpeed?.Invoke(CurrentSpeed);
    }
}