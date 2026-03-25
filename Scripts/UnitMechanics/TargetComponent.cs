using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class TargetComponent : MonoBehaviour
{
    [SerializeField] private StatsComponent _statsComponent;

    [field: SerializeField] public GameObject CurrentTarget { get; private set; }

    public StatsComponent TargetStats => CurrentTarget != null ? CurrentTarget.GetComponent<StatsComponent>() : null;

    [SerializeField] public UnityEvent<GameObject> OnTargetSet;
    [SerializeField] public UnityEvent OnTargetCleared;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void SetTarget(GameObject target)
    {
        if (CurrentTarget == target) return;
        CurrentTarget = target;
        OnTargetSet?.Invoke(target);
    }

    public void ClearTarget()
    {
        SetTarget(null);
        OnTargetCleared?.Invoke();
    }
}
