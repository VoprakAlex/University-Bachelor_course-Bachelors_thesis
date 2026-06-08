using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ActionComponent))]
public class TargetComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ActionComponent _actionComponent;

    [Header("Target")]
    [field: SerializeField] public GameObject MainTarget { get; private set; }
    [SerializeField] public List<GameObject> SubTargets = new List<GameObject>();

    [Header("TargetEvents")]
    public UnityEvent<GameObject> OnMainTargetSet;
    public UnityEvent OnSubTargetsSet;
    public UnityEvent OnMainTargetCleared;
    public UnityEvent OnSubTargetCleared;

    private void Awake()
    {
        if (_actionComponent == null)
            _actionComponent = GetComponent<ActionComponent>();
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

    public void ClearTargets()
    {
        ClearMainTarget();
        ClearSubTargets();
    }

    public void RemoveFirstSubTarget()
    {
        if (SubTargets.Count > 0)
        {
            SubTargets.RemoveAt(0);
        }
    }

    public void ClearMainTarget()
    {
        MainTarget = null;
        SubTargets.Clear();

        OnMainTargetCleared?.Invoke();
    }

    public void ClearSubTargets()
    {
        SubTargets.Clear();

        OnSubTargetCleared?.Invoke();
    }
}