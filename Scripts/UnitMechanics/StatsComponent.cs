using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public enum DamageType
{
    Slashing = 0,
    Piercing = 1,
    Bludgening = 2,
}

public enum DamageAffinity
{
    Ruby = 0,       // HP
    Sapphire = 1,   // SP
    Topaz = 2,      // Stagger
    Garnet = 3,     // HP + Stagger
    Amethyst = 4,   // HP + SP
    Diamond = 5     // все
}

public enum ResistanceTier
{
    Immune = 0,       // multiplier 0.1f 
    Ineffective = 1,  // multiplier 0.5f
    Resilient = 2,    // multiplier 0.75f
    Normal = 3,       // multiplier 1.0f
    Vulnerable = 4,   // multiplier 1.25f
    Effective = 5,    // multiplier 1.5f
    Fatal = 6         // multiplier 2.0f
}

public static class ResistanceData
{
    public static float GetMultiplier(this ResistanceTier tier)
    {
        return tier switch
        {
            ResistanceTier.Immune => 0.1f,
            ResistanceTier.Ineffective => 0.5f,
            ResistanceTier.Resilient => 0.75f,
            ResistanceTier.Normal => 1.0f,
            ResistanceTier.Vulnerable => 1.25f,
            ResistanceTier.Effective => 1.5f,
            ResistanceTier.Fatal => 2.0f,
            _ => 1.0f,
        };
    }
}
[RequireComponent(typeof(HealthComponent), typeof(StaggerComponent))]
public class StatsComponent : MonoBehaviour
{

    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private StaggerComponent _staggerComponent;
    [SerializeField] private SanityComponent _sanityComponent;
    [SerializeField] private ResistanceComponent _resistanceComponent;
    [SerializeField] private SpeedComponent _speedComponent;
    [SerializeField] private TargetComponent _targetComponent;

    public int MaxHP;
    public int MaxSP;

    public float MinSpeed;
    public float MaxSpeed;

    public float StaggerPercent;

    public DamageType StandardDamageType = DamageType.Slashing;

    public int BaseAttackPower;

    [SerializedDictionary("DamageType", "ResistanceTier")]
    public SerializedDictionary<DamageType, ResistanceTier> StandardDamageTypeResistances = new SerializedDictionary<DamageType, ResistanceTier>
    {
        { DamageType.Slashing, ResistanceTier.Normal },
        { DamageType.Piercing, ResistanceTier.Normal },
        { DamageType.Bludgening, ResistanceTier.Normal }
    };

    [SerializedDictionary("DamageAffinity", "ResistanceTier")]
    public SerializedDictionary<DamageAffinity, ResistanceTier> StandardDamageAffinityResistances = new SerializedDictionary<DamageAffinity, ResistanceTier>
    {
        { DamageAffinity.Ruby, ResistanceTier.Normal },
        { DamageAffinity.Sapphire, ResistanceTier.Normal },
        { DamageAffinity.Topaz, ResistanceTier.Normal },
        { DamageAffinity.Garnet, ResistanceTier.Normal },
        { DamageAffinity.Amethyst, ResistanceTier.Normal },
        { DamageAffinity.Diamond, ResistanceTier.Normal }
    };

    private void Awake()
    {
        if (_healthComponent == null)
            _healthComponent = GetComponent<HealthComponent>();
        if (_staggerComponent == null)
            _staggerComponent = GetComponent<StaggerComponent>();
        if (_sanityComponent == null)
            _sanityComponent = GetComponent<SanityComponent>();
        if (_resistanceComponent == null)
            _resistanceComponent = GetComponent<ResistanceComponent>();
    }

    void Start()
    {
        _healthComponent.SetToMax();
        _sanityComponent.SetToMax();
        _resistanceComponent.SetResistanceToStandard();
    }


    public float GetDamageMultiplier(DamageType damageType, DamageAffinity affinity)
    {
        ResistanceTier typeTier = _resistanceComponent.CurrentDamageTypeResistances[damageType];
        ResistanceTier affinityTier = _resistanceComponent.CurrentDamageAffinityResistances[affinity];
        return typeTier.GetMultiplier() * affinityTier.GetMultiplier();
    }


}
