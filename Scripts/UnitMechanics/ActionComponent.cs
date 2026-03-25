using UnityEngine;

[RequireComponent(typeof(StatsComponent))]
public class ActionComponent : MonoBehaviour
{
    [SerializeField] private StatsComponent _statsComponent;

    public DamageType CurrentDamageType { get; private set; }
    public int CurrentAttackPower { get; private set; }

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void ResetToBase()
    {
        CurrentDamageType = _statsComponent.StandardDamageType;
        CurrentAttackPower = _statsComponent.BaseAttackPower;
    }
}