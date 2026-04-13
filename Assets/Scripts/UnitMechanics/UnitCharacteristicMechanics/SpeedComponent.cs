using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class SpeedComponent : MonoBehaviour
{
    [SerializeField] private StatsComponent _statsComponent;

    [field: SerializeField] public float CurrentSpeed { get; private set; }

    [SerializeField] public UnityEvent<float> OnSetSpeed;
    [SerializeField] public UnityEvent<float> OnIncreaseSpeed;
    [SerializeField] public UnityEvent<float> OnDecreaseSpeed;

    private void Awake()
    {
        if (_statsComponent == null)
        {
            _statsComponent = GetComponent<StatsComponent>();
        }
    }

    public void SetSpeed(float newSpeed)
    {
        CurrentSpeed = Mathf.Max(newSpeed, 1f);
        OnSetSpeed?.Invoke(newSpeed);
    }

    public void SetRandomSpeed()
    {
        float randomSpeed = Random.Range(_statsComponent.MinSpeed, _statsComponent.MaxSpeed);
        SetSpeed(randomSpeed);
    }

    public void IncreaseSpeed(float amount)
    {
        SetSpeed(CurrentSpeed + amount);
        OnIncreaseSpeed?.Invoke(CurrentSpeed);
    }

    public void DecreaseSpeed(float amount)
    {
        SetSpeed(CurrentSpeed - amount);
        OnDecreaseSpeed?.Invoke(CurrentSpeed);
    }
}