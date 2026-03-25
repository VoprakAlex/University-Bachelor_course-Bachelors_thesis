using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent), typeof(StaggerComponent))]
public class ResistanceComponent : MonoBehaviour
{
    [SerializeField] private StatsComponent _statsComponent;
    [SerializeField] private StaggerComponent _staggerComponent;

    [field: SerializedDictionary("DamageType", "ResistanceTier")]
    public SerializedDictionary<DamageType, ResistanceTier> CurrentDamageTypeResistances //{ get; private set; }
        = new SerializedDictionary<DamageType, ResistanceTier>();

    [field: SerializedDictionary("DamageAffinity", "ResistanceTier")]
    public SerializedDictionary<DamageAffinity, ResistanceTier> CurrentDamageAffinityResistances //{ get; private set; }
        = new SerializedDictionary<DamageAffinity, ResistanceTier>();


    [SerializeField] public UnityEvent OnSetDamageTypeResistancesToMax;
    [SerializeField] public UnityEvent OnSetDamageTypeResistancesToStandard;
    [SerializeField] public UnityEvent<DamageType, ResistanceTier> OnDamageTypeResistanceChanged;
    [SerializeField] public UnityEvent<DamageAffinity, ResistanceTier> OnDamageAffinityResistanceChanged;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
        if (_staggerComponent == null)
            _staggerComponent = GetComponent<StaggerComponent>();
    }

    private void OnEnable()
    {
        _staggerComponent.OnStagger.AddListener(SetDamageTypeResistancesToMax);
        _staggerComponent.OnUnstagger.AddListener(SetDamageTypeResistancesToStandard);
    }

    private void OnDisable()
    {
        _staggerComponent.OnStagger.RemoveListener(SetDamageTypeResistancesToMax);
        _staggerComponent.OnUnstagger.RemoveListener(SetDamageTypeResistancesToStandard);
    }

    public void SetResistance(DamageType type, ResistanceTier tier)
    {
        CurrentDamageTypeResistances[type] = tier;
        OnDamageTypeResistanceChanged?.Invoke(type, tier);
    }

    public void SetResistance(DamageAffinity affinity, ResistanceTier tier)
    {
        CurrentDamageAffinityResistances[affinity] = tier;
        OnDamageAffinityResistanceChanged?.Invoke(affinity, tier);
    }

    public void SetAllResistanceTypes(ResistanceTier tier)
    {
        foreach (var type in System.Enum.GetValues(typeof(DamageType)))
        {
            DamageType dt = (DamageType)type;
            SetResistance(dt, tier);
        }
    }

    public void SetAllResistanceAffinity(ResistanceTier tier)
    {
        foreach (var affinity in System.Enum.GetValues(typeof(DamageAffinity)))
        {
            DamageAffinity da = (DamageAffinity)affinity;
            SetResistance(da, tier);
        }
    }

    public void SetDamageTypeResistancesToMax()
    {
        SetAllResistanceTypes(ResistanceTier.Fatal);
        OnSetDamageTypeResistancesToMax?.Invoke();
    }

    public void SetDamageAffinityResistancesToMax()
    {
        SetAllResistanceAffinity(ResistanceTier.Fatal);
    }

    public void SetDamageTypeResistancesToStandard()
    {
        foreach (var kvp in _statsComponent.StandardDamageTypeResistances)
            SetResistance(kvp.Key, kvp.Value);
        OnSetDamageTypeResistancesToStandard?.Invoke();
    }

    public void SetDamageAffinityResistancesToStandard()
    {
        foreach (var kvp in _statsComponent.StandardDamageAffinityResistances)
            SetResistance(kvp.Key, kvp.Value);
    }

    public void SetResistanceToStandard()
    {
        SetDamageTypeResistancesToStandard();
        SetDamageAffinityResistancesToStandard();
    }

    public void IncreaseResistance(DamageType type)
    {
        ResistanceTier currentTier = CurrentDamageTypeResistances[type];
        int nextTierValue = (int)currentTier + 1;

        if (nextTierValue < System.Enum.GetValues(typeof(ResistanceTier)).Length)
        {
            SetResistance(type, (ResistanceTier)nextTierValue);
        }
    }

    public void IncreaseResistance(DamageAffinity affinity)
    {
        ResistanceTier currentTier = CurrentDamageAffinityResistances[affinity];
        int nextTierValue = (int)currentTier + 1;

        if (nextTierValue < System.Enum.GetValues(typeof(ResistanceTier)).Length)
        {
            SetResistance(affinity, (ResistanceTier)nextTierValue);
        }
    }

    public void DecreaseResistance(DamageType type)
    {
        ResistanceTier currentTier = CurrentDamageTypeResistances[type];
        int nextTierValue = (int)currentTier - 1;

        if (nextTierValue >= 0)
        {
            SetResistance(type, (ResistanceTier)nextTierValue);
        }
    }

    public void DecreaseResistance(DamageAffinity affinity)
    {
        ResistanceTier currentTier = CurrentDamageAffinityResistances[affinity];
        int nextTierValue = (int)currentTier - 1;

        if (nextTierValue >= 0)
        {
            SetResistance(affinity, (ResistanceTier)nextTierValue);
        }
    }

    public void IncreaseAllDamageTypeResistances()
    {
        foreach (DamageType type in System.Enum.GetValues(typeof(DamageType)))
        {
            IncreaseResistance(type);
        }
    }

    public void IncreaseAllDamageAffinityResistances()
    {
        foreach (DamageAffinity affinity in System.Enum.GetValues(typeof(DamageAffinity)))
        {
            IncreaseResistance(affinity);
        }
    }

    public void DecreaseAllDamageTypeResistances()
    {
        foreach (DamageType type in System.Enum.GetValues(typeof(DamageType)))
        {
            DecreaseResistance(type);
        }
    }

    public void DecreaseAllDamageAffinityResistances()
    {
        foreach (DamageAffinity affinity in System.Enum.GetValues(typeof(DamageAffinity)))
        {
            DecreaseResistance(affinity);
        }
    }
}
