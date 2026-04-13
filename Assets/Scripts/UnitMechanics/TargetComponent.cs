using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class TargetComponent : MonoBehaviour
{
    [SerializeField] private StatsComponent _statsComponent;

    [field: SerializeField] public GameObject ClashTarget { get; private set; }
    [field: SerializeField] public GameObject MainTarget { get; private set; }

    public List<GameObject> SubTargets = new List<GameObject>();

    [SerializeField] public UnityEvent<GameObject> OnMainTargetSet;
    [SerializeField] public UnityEvent OnSubTargetsSet;
    [SerializeField] public UnityEvent OnTargetCleared;
    [SerializeField] public UnityEvent<GameObject> OnClashTargetSet;
    [SerializeField] public UnityEvent OnClashTargetCleared;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void SetMainTarget(GameObject target)
    {
        if (MainTarget == target) return;
        MainTarget = target;
        OnMainTargetSet?.Invoke(target);
    }

    public void FillSubTargets(int count, List<GameObject> possibleTargets)
    {
        if (count <= 0) return;
        if (possibleTargets == null)
        {
            SubTargets.Clear();
            return;
        }

        List<GameObject> availableTargets = new List<GameObject>(possibleTargets);

        if (MainTarget != null && availableTargets.Contains(MainTarget))
            availableTargets.Remove(MainTarget);

        int takeCount = Mathf.Min(count, availableTargets.Count);

        if (takeCount <= 0)
        {
            SubTargets.Clear();
            return;
        }

        List<GameObject> selected = availableTargets.GetRange(availableTargets.Count - takeCount, takeCount);

        SubTargets.Clear();
        SubTargets.AddRange(selected);
        OnSubTargetsSet?.Invoke();
    }

    public void SetClashTarget(GameObject target)
    {
        if (ClashTarget == target) return;

        ClashTarget = target;
        OnClashTargetSet?.Invoke(target);
    }

    public void ClearClashTarget()
    {
        if (ClashTarget == null) return;

        ClashTarget = null;
        OnClashTargetCleared?.Invoke();
    }

    public void ClearTargets()
    {
        MainTarget = null;
        SubTargets.Clear();
        ClearClashTarget();

        OnTargetCleared?.Invoke();
    }

    public void RemoveFirstSubTarget()
    {
        if (SubTargets.Count > 0)
        {
            SubTargets.RemoveAt(0);
        }
    }

}