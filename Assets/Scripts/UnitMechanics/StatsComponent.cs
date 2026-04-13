using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(HealthComponent), typeof(StaggerComponent))]
public class StatsComponent : MonoBehaviour
{

    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private ShieldComponent _shieldComponent;
    [SerializeField] private StaggerComponent _staggerComponent;
    [SerializeField] private SanityComponent _sanityComponent;
    [SerializeField] private ResistanceComponent _resistanceComponent;
    [SerializeField] private SpeedComponent _speedComponent;
    [SerializeField] private TargetComponent _targetComponent;

    public CharacterData Character { get; private set; }

    public int MaxHP;
    public int MaxSP;

    public int StartingShield;

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
        if (_shieldComponent == null)
            _shieldComponent = GetComponent<ShieldComponent>();
        if (_staggerComponent == null)
            _staggerComponent = GetComponent<StaggerComponent>();
        if (_sanityComponent == null)
            _sanityComponent = GetComponent<SanityComponent>();
        if (_resistanceComponent == null)
            _resistanceComponent = GetComponent<ResistanceComponent>();
        if (_speedComponent == null)
            _speedComponent = GetComponent<SpeedComponent>();
    }

    void Start()
    {
        MaxHP = Character.MaxHP;
        MaxSP = Character.MaxSP;

        StartingShield = Character.StartingShield;

        MinSpeed = Character.MinSpeed;
        MaxSpeed = Character.MaxSpeed;

        StaggerPercent = Character.StaggerPercent;

        StandardDamageTypeResistances = new SerializedDictionary<DamageType, ResistanceTier>(Character.StandardDamageTypeResistances);
        StandardDamageAffinityResistances = new SerializedDictionary<DamageAffinity, ResistanceTier>(Character.StandardDamageAffinityResistances);

        _healthComponent.SetToMax();
        _sanityComponent.SetToMax();
        _shieldComponent.SetToStarting();
        _resistanceComponent.SetResistanceToStandard();
    }


    public float GetDamageMultiplier(DamageType damageType, DamageAffinity affinity)
    {
        ResistanceTier typeTier = _resistanceComponent.CurrentDamageTypeResistances[damageType];
        ResistanceTier affinityTier = _resistanceComponent.CurrentDamageAffinityResistances[affinity];
        return typeTier.GetMultiplier() * affinityTier.GetMultiplier();
    }


}
